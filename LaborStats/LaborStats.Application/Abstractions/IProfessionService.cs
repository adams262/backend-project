using LaborStats.Application.Professions;

namespace LaborStats.Application.Abstractions;

public interface IProfessionService
{
    Task<ProfessionResponse> CreateAsync(
        CreateProfessionRequest request,
        CancellationToken cancellationToken = default);

    Task<ProfessionResponse> UpdateAsync(
        Guid id,
        UpdateProfessionRequest request,
        CancellationToken cancellationToken = default);

    Task<ProfessionDetailResponse?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ProfessionResponse>> GetAllAsync(
        CancellationToken cancellationToken = default);
}
