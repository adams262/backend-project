using AutoMapper;
using AutoMapper.QueryableExtensions;
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
    TimeProvider timeProvider,
    IMapper mapper) : IProfessionGroupService
{
    public async Task<ProfessionGroupResponse> CreateAsync(
        CreateProfessionGroupRequest request,
        CancellationToken cancellationToken = default)
    {
        if (await context.ProfessionGroup.AnyAsync(pg => pg.Name == request.Name, cancellationToken))
        {
            throw new ConflictException($"Profession group '{request.Name}' already exists.");
        }

        var group = mapper.Map<ProfessionGroups>(request);

        var (userId, username) = currentUserService.RequireAuditUser();
        var now = timeProvider.GetUtcNow();
        group.CreatedAt = now;
        group.CreatedById = userId;
        group.CreatedByName = username;

        context.ProfessionGroup.Add(group);
        await context.SaveChangesAsync(cancellationToken);

        return mapper.Map<ProfessionGroupResponse>(group);
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
            if (await context.ProfessionGroup.AnyAsync(pg => pg.Name == request.Name && pg.Id != id, cancellationToken))
            {
                throw new ConflictException($"Profession group '{request.Name}' already exists.");
            }
        }

        mapper.Map(request, group);

        var (userId, username) = currentUserService.RequireAuditUser();
        group.UpdatedAt = timeProvider.GetUtcNow();
        group.UpdatedById = userId;
        group.UpdatedByName = username;

        await context.SaveChangesAsync(cancellationToken);

        return mapper.Map<ProfessionGroupResponse>(group);
    }

    public async Task<ProfessionGroupDetailResponse?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await context.ProfessionGroup
            .Where(pg => pg.Id == id)
            .ProjectTo<ProfessionGroupDetailResponse>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ProfessionGroupResponse>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await context.ProfessionGroup
            .OrderBy(pg => pg.Name)
            .ProjectTo<ProfessionGroupResponse>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}
