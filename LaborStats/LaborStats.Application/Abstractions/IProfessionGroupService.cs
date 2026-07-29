using LaborStats.Application.ProfessionGroups;

namespace LaborStats.Application.Abstractions;

public interface IProfessionGroupService
{
    Task<ProfessionGroupResponse> CreateAsync(
        CreateProfessionGroupRequest request,
        CancellationToken cancellationToken = default);

    Task<ProfessionGroupResponse> UpdateAsync(
        Guid id,
        UpdateProfessionGroupRequest request,
        CancellationToken cancellationToken = default);

    Task<ProfessionGroupDetailResponse?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ProfessionGroupResponse>> GetAllAsync(
        CancellationToken cancellationToken = default);
}
