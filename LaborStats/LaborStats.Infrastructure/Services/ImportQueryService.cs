using LaborStats.Application.Abstractions;
using LaborStats.Application.Imports;
using LaborStats.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LaborStats.Infrastructure.Services;

public sealed class ImportQueryService(LaborStatsDbContext context) : IImportQueryService
{
    public async Task<IReadOnlyList<ImportListItemResponse>>
        GetAllAsync(
            CancellationToken cancellationToken = default)
    {
        var imports = await context.ImportHistories
            .AsNoTracking()
            .OrderByDescending(importHistory =>
                importHistory.ImportEndDate)
            .Select(importHistory => new
            {
                importHistory.Id,
                importHistory.FileName,
                importHistory.Voivodeship,
                importHistory.Year,
                importHistory.Period,
                importHistory.ImportEndDate,
                importHistory.ProcessedRecordsCount
            })
            .ToListAsync(cancellationToken);

        return
        [
            .. imports.Select(importHistory =>
                new ImportListItemResponse(
                    importHistory.Id,
                    importHistory.FileName,
                    importHistory.Voivodeship,
                    importHistory.Year,
                    importHistory.Period.ToString(),
                    importHistory.ImportEndDate,
                    importHistory.ProcessedRecordsCount))
        ];
    }

    public async Task<ImportDetailsResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var importHistory = await context.ImportHistories
            .AsNoTracking()
            .Where(importHistory =>
                importHistory.Id == id)
            .Select(importHistory => new
            {
                importHistory.Id,
                importHistory.FileName,
                importHistory.Voivodeship,
                importHistory.Year,
                importHistory.Period,
                importHistory.ImportEndDate,
                importHistory.ProcessedRecordsCount,
                importHistory.CreatedBy
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (importHistory is null)
        {
            return null;
        }

        return new ImportDetailsResponse(
            importHistory.Id,
            importHistory.FileName,
            importHistory.Voivodeship,
            importHistory.Year,
            importHistory.Period.ToString(),
            importHistory.ImportEndDate,
            importHistory.ProcessedRecordsCount,
            importHistory.CreatedBy);
    }

    public async Task<IReadOnlyList<LaborStatRecordResponse>?>
        GetRecordsAsync(
            Guid importId,
            CancellationToken cancellationToken = default)
    {
        bool importExists =
            await context.ImportHistories
                .AsNoTracking()
                .AnyAsync(
                    importHistory =>
                        importHistory.Id == importId,
                    cancellationToken);

        if (!importExists)
        {
            return null;
        }

        return await context.LaborStatRecords
            .AsNoTracking()
            .Where(record =>
                record.ImportHistoryId == importId)
            .OrderBy(record => record.County)
            .ThenBy(record => record.OccupationCode)
            .Select(record =>
                new LaborStatRecordResponse(
                    record.Id,
                    record.ImportHistoryId,
                    record.Voivodeship,
                    record.County,
                    record.OccupationCode,
                    record.InsuranceTitleCode,
                    record.TotalContractsCount,
                    record.LongTermContractsCount,
                    record.NewlyRegisteredCount,
                    record.DataType))
            .ToListAsync(cancellationToken);
    }
}
