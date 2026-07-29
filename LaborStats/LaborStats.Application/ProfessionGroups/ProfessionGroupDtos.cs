namespace LaborStats.Application.ProfessionGroups;

public sealed record CreateProfessionGroupRequest(
    string Name,
    string? NameEn);

public sealed record UpdateProfessionGroupRequest(
    string? Name,
    string? NameEn);

public sealed record ProfessionGroupResponse(
    Guid Id,
    string Name,
    string? NameEn);

public sealed record ProfessionGroupDetailResponse(
    Guid Id,
    string Name,
    string? NameEn,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt,
    string? CreatedByName,
    string? UpdatedByName,
    IReadOnlyList<ProfessionGroupProfessionResponse> Professions);

public sealed record ProfessionGroupProfessionResponse(
    Guid Id,
    int KzisCode,
    string KzisName);
