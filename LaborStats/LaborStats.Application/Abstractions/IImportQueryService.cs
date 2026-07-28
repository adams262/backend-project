using LaborStats.Application.Imports;

namespace LaborStats.Application.Abstractions;

public interface IImportQueryService
{
    Task<IReadOnlyList<ImportListItemResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ImportDetailsResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<LaborStatRecordResponse>?> GetRecordsAsync(Guid importId,CancellationToken cancellationToken = default);
}
