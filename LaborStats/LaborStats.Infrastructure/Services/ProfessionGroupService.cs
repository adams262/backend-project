using LaborStats.Application.Abstractions;
using LaborStats.Application.ProfessionGroups;
using LaborStats.Domain.Entities;
using LaborStats.Domain.Exceptions;
using LaborStats.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LaborStats.Infrastructure.Services;

public sealed class ProfessionGroupService(
    LaborStatsDbContext context,
    ICurrentUserService currentUserService,
    TimeProvider timeProvider) : IProfessionGroupService
{
    public async Task<ProfessionGroupResponse> CreateAsync(
        CreateProfessionGroupRequest request,
        CancellationToken cancellationToken = default)
    {
        bool exists = await context.ProfessionGroup
            .AnyAsync(pg => pg.Name == request.Name, cancellationToken);

        if (exists)
        {
            throw new ConflictException($"Profession group '{request.Name}' already exists.");
        }

        var (userId, username) = currentUserService.RequireAuditUser();
        var now = timeProvider.GetUtcNow();

        var group = new ProfessionGroups
        {
            Name = request.Name,
            NameEn = request.NameEn,
            CreatedAt = now,
            CreatedById = userId,
            CreatedByName = username
        };

        context.ProfessionGroup.Add(group);
        await context.SaveChangesAsync(cancellationToken);

        return new ProfessionGroupResponse(group.Id, group.Name, group.NameEn);
    }

    public async Task<ProfessionGroupResponse> UpdateAsync(
        Guid id,
        UpdateProfessionGroupRequest request,
        CancellationToken cancellationToken = default)
    {
        var group = await context.ProfessionGroup
            .FirstOrDefaultAsync(pg => pg.Id == id, cancellationToken);

        if (group is null)
        {
            throw new NotFoundException(nameof(ProfessionGroups), id);
        }

        if (request.Name is not null && request.Name != group.Name)
        {
            bool duplicate = await context.ProfessionGroup
                .AnyAsync(pg => pg.Name == request.Name && pg.Id != id, cancellationToken);

            if (duplicate)
            {
                throw new ConflictException($"Profession group '{request.Name}' already exists.");
            }

            group.Name = request.Name;
        }

        if (request.NameEn is not null)
        {
            group.NameEn = request.NameEn;
        }

        var (userId, username) = currentUserService.RequireAuditUser();
        group.UpdatedAt = timeProvider.GetUtcNow();
        group.UpdatedById = userId;
        group.UpdatedByName = username;

        await context.SaveChangesAsync(cancellationToken);

        return new ProfessionGroupResponse(group.Id, group.Name, group.NameEn);
    }

    public async Task<ProfessionGroupDetailResponse?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await context.ProfessionGroup
            .Where(pg => pg.Id == id)
            .Select(pg => new ProfessionGroupDetailResponse(
                pg.Id,
                pg.Name,
                pg.NameEn,
                pg.CreatedAt,
                pg.UpdatedAt,
                pg.CreatedByName,
                pg.UpdatedByName,
                pg.Professions
                    .OrderBy(p => p.KzisCode)
                    .Select(p => new ProfessionGroupProfessionResponse(p.Id, p.KzisCode, p.KzisName))
                    .ToList()))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ProfessionGroupResponse>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await context.ProfessionGroup
            .OrderBy(pg => pg.Name)
            .Select(pg => new ProfessionGroupResponse(pg.Id, pg.Name, pg.NameEn))
            .ToListAsync(cancellationToken);
    }
}
