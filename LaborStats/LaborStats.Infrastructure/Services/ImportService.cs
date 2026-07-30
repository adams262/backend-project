using LaborStats.Application.Abstractions;
using LaborStats.Application.Imports;
using LaborStats.Application.Imports.Dtos;
using LaborStats.Domain.Entities;
using LaborStats.Infrastructure.Data;

namespace LaborStats.Infrastructure.Services;

public class ImportService(
    IImportValidator importValidator,
    IImportIdempotencyChecker idempotencyChecker,
    IDataConversionService dataConversionService,
    LaborStatsDbContext dbContext) : IImportService
{

    private const string InsuredAllContracts = "INSURED_ALL_CONTRACTS";
    private const string InsuredOver2Years = "INSURED_OVER_2_YEARS";
    private const string InsuredNewlyRegistered = "INSURED_NEWLY_REGISTERED";
    public async Task ProcessImportAsync(
        ImportRequestDto request,
        string username,
        CancellationToken cancellationToken)
    {
        await idempotencyChecker.EnsureNotAlreadyImportedAsync(
            request.Voivodeship, request.Year, request.Period, request.DataType, cancellationToken);

        var validationResult = await importValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ImportValidationException(validationResult.Errors);
        }

        var convertedRows = await ConvertFileAsync(request, cancellationToken);
        var records = BuildLaborStatRecords(convertedRows, request);
        var importHistory = BuildImportHistory(request, username, records.Count);

        await SaveImportAsync(importHistory, records, cancellationToken);
    }

    private async Task<List<ConvertedImportRowDto>> ConvertFileAsync(
        ImportRequestDto request,
        CancellationToken cancellationToken)
    {
        using var stream = request.File.OpenReadStream();
        return await dataConversionService.ConvertAsync(stream, request.Voivodeship, request.DataType, cancellationToken);
    }

    private static List<LaborStatRecord> BuildLaborStatRecords(
        List<ConvertedImportRowDto> convertedRows,
        ImportRequestDto request)
    {
        string dataTypeValue = request.DataType.ToString();

        return convertedRows
            .GroupBy(r => (r.CountyId, r.ProfessionId))
            .Select(g => new LaborStatRecord
            {
                Id = Guid.CreateVersion7(),
                Voivodeship = request.Voivodeship,
                County = g.Key.CountyId,
                OccupationCode = g.Key.ProfessionId.ToString(),
                InsuranceTitleCode = string.Empty, 
                DataType = dataTypeValue,
                TotalContractsCount = g.FirstOrDefault(r => r.DataType == InsuredAllContracts)?.Value,
                LongTermContractsCount = g.FirstOrDefault(r => r.DataType == InsuredOver2Years)?.Value,
                NewlyRegisteredCount = g.FirstOrDefault(r => r.DataType == InsuredNewlyRegistered)?.Value
            })
            .ToList();
    }

    private static ImportHistory BuildImportHistory(
        ImportRequestDto request,
        string username,
        int processedRecordsCount)
    {
        return new ImportHistory
        {
            Id = Guid.CreateVersion7(),
            FileName = request.File.FileName,
            Voivodeship = request.Voivodeship,
            Year = request.Year,
            Period = request.Period,
            ImportEndDate = DateTime.UtcNow,
            ProcessedRecordsCount = processedRecordsCount,
            CreatedBy = username
        };
    }

    private async Task SaveImportAsync(
        ImportHistory importHistory,
        List<LaborStatRecord> records,
        CancellationToken cancellationToken)
    {
        dbContext.ImportHistories.Add(importHistory);

        foreach (var record in records)
        {
            record.ImportHistory = importHistory;
        }

        dbContext.LaborStatRecords.AddRange(records);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
