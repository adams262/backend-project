namespace LaborStats.Application.Professions;

public sealed record CreateProfessionRequest(
    int KzisCode,
    string KzisName,
    Guid ProfessionGroupId);

public sealed record UpdateProfessionRequest(
    int? KzisCode,
    string? KzisName,
    Guid? ProfessionGroupId);

public sealed record ProfessionResponse(
    Guid Id,
    int KzisCode,
    string KzisName,
    Guid ProfessionGroupId,
    string ProfessionGroupName);

public sealed record ProfessionDetailResponse(
    Guid Id,
    int KzisCode,
    string KzisName,
    Guid ProfessionGroupId,
    string ProfessionGroupName,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt,
    string? CreatedByName,
    string? UpdatedByName);
