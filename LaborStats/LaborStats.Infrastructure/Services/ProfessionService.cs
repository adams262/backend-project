using LaborStats.Application.Abstractions;
using LaborStats.Application.Professions;
using LaborStats.Domain.Entities;
using LaborStats.Domain.Exceptions;
using LaborStats.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LaborStats.Infrastructure.Services;

public sealed class ProfessionService(
    LaborStatsDbContext context,
    ICurrentUserService currentUserService,
    TimeProvider timeProvider) : IProfessionService
{
    public async Task<ProfessionResponse> CreateAsync(
        CreateProfessionRequest request,
        CancellationToken cancellationToken = default)
    {
        var group = await context.ProfessionGroup
            .Where(pg => pg.Id == request.ProfessionGroupId)
            .Select(pg => new { pg.Id, pg.Name })
            .FirstOrDefaultAsync(cancellationToken);

        if (group is null)
        {
            throw new NotFoundException(nameof(ProfessionGroups), request.ProfessionGroupId);
        }

        bool duplicate = await context.Profession
            .AnyAsync(p => p.KzisCode == request.KzisCode, cancellationToken);

        if (duplicate)
        {
            throw new ConflictException($"Profession with KzisCode {request.KzisCode} already exists.");
        }

        var (userId, username) = currentUserService.RequireAuditUser();
        var now = timeProvider.GetUtcNow();

        var profession = new Professions
        {
            KzisCode = request.KzisCode,
            KzisName = request.KzisName,
            ProfessionGroupId = request.ProfessionGroupId,
            CreatedAt = now,
            CreatedById = userId,
            CreatedByName = username
        };

        context.Profession.Add(profession);
        await context.SaveChangesAsync(cancellationToken);

        return new ProfessionResponse(profession.Id,
            profession.KzisCode,
            profession.KzisName,
            profession.ProfessionGroupId,
            group.Name);
    }

    public async Task<ProfessionResponse> UpdateAsync(
        Guid id,
        UpdateProfessionRequest request,
        CancellationToken cancellationToken = default)
    {
        var profession = await context.Profession
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

        if (profession is null)
        {
            throw new NotFoundException(nameof(Professions), id);
        }

        string? groupName = null;

        if (request.KzisCode is not null && request.KzisCode != profession.KzisCode)
        {
            int newKzisCode = request.KzisCode.Value;
            bool duplicate = await context.Profession
                .AnyAsync(p => p.KzisCode == newKzisCode && p.Id != id, cancellationToken);

            if (duplicate)
            {
                throw new ConflictException($"Profession with KzisCode {request.KzisCode} already exists.");
            }

            profession.KzisCode = newKzisCode;
        }

        if (request.KzisName is not null)
        {
            profession.KzisName = request.KzisName;
        }

        if (request.ProfessionGroupId is not null && request.ProfessionGroupId != profession.ProfessionGroupId)
        {
            Guid newGroupId = request.ProfessionGroupId.Value;
            var newGroup = await context.ProfessionGroup
                .Where(pg => pg.Id == newGroupId)
                .Select(pg => new { pg.Id, pg.Name })
                .FirstOrDefaultAsync(cancellationToken);

            if (newGroup is null)
            {
                throw new NotFoundException(nameof(ProfessionGroups), newGroupId);
            }

            profession.ProfessionGroupId = newGroupId;
            groupName = newGroup.Name;
        }

        var (userId, username) = currentUserService.RequireAuditUser();
        profession.UpdatedAt = timeProvider.GetUtcNow();
        profession.UpdatedById = userId;
        profession.UpdatedByName = username;

        await context.SaveChangesAsync(cancellationToken);

        groupName ??= await context.ProfessionGroup
            .Where(pg => pg.Id == profession.ProfessionGroupId)
            .Select(pg => pg.Name)
            .FirstOrDefaultAsync(cancellationToken);

        if (groupName is null)
        {
            throw new NotFoundException(nameof(ProfessionGroups), profession.ProfessionGroupId);
        }

        return new ProfessionResponse(profession.Id,
            profession.KzisCode,
            profession.KzisName,
            profession.ProfessionGroupId,
            groupName);
    }

    public async Task<ProfessionDetailResponse?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await context.Profession
            .Where(p => p.Id == id)
            .Select(p => new ProfessionDetailResponse(
                p.Id,
                p.KzisCode,
                p.KzisName,
                p.ProfessionGroupId,
                p.ProfessionGroup.Name,
                p.CreatedAt,
                p.UpdatedAt,
                p.CreatedByName,
                p.UpdatedByName))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ProfessionResponse>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await context.Profession
            .OrderBy(p => p.KzisName)
            .Select(p => new ProfessionResponse(p.Id,
                p.KzisCode,
                p.KzisName,
                p.ProfessionGroupId,
                p.ProfessionGroup.Name))
            .ToListAsync(cancellationToken);
    }
}
