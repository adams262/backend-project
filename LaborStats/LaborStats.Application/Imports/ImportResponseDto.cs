namespace LaborStats.Application.Imports;

public sealed record ImportListItemResponse(
    Guid Id,
    string FileName,
    string Voivodeship,
    int Year,
    string Period,
    DateTime ImportEndDate,
    int ProcessedRecordsCount);

public sealed record ImportDetailsResponse(
    Guid Id,
    string FileName,
    string Voivodeship,
    int Year,
    string Period,
    DateTime ImportEndDate,
    int ProcessedRecordsCount,
    string CreatedBy);

public sealed record LaborStatRecordResponse(
    Guid Id,
    Guid ImportHistoryId,
    string Voivodeship,
    string County,
    string OccupationCode,
    string InsuranceTitleCode,
    int? TotalContractsCount,
    int? LongTermContractsCount,
    int? NewlyRegisteredCount,
    string DataType);
