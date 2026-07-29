using LaborStats.Application.Imports;
using LaborStats.Domain.Entities;

namespace LaborStats.Infrastructure.Services;

internal static class ImportMapper
{
    internal static ImportListItemResponse ToListItemResponse(
        this ImportHistory importHistory)
    {
        return new ImportListItemResponse(
            importHistory.Id,
            importHistory.FileName,
            importHistory.Voivodeship,
            importHistory.Year,
            importHistory.Period.ToString(),
            importHistory.ImportEndDate,
            importHistory.ProcessedRecordsCount);
    }

    internal static ImportDetailsResponse ToDetailsResponse(
        this ImportHistory importHistory)
    {
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

    internal static LaborStatRecordResponse ToResponse(
        this LaborStatRecord record)
    {
        return new LaborStatRecordResponse(
            record.Id,
            record.ImportHistoryId,
            record.Voivodeship,
            record.County,
            record.OccupationCode,
            record.InsuranceTitleCode,
            record.TotalContractsCount,
            record.LongTermContractsCount,
            record.NewlyRegisteredCount,
            record.DataType);
    }
}
