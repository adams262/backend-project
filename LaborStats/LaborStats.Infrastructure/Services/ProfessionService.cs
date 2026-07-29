using AutoMapper;
using AutoMapper.QueryableExtensions;
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
    TimeProvider timeProvider,
    IMapper mapper) : IProfessionService
{
    public async Task<ProfessionResponse> CreateAsync(
        CreateProfessionRequest request,
        CancellationToken cancellationToken = default)
    {
        if (!await context.ProfessionGroup.AnyAsync(pg => pg.Id == request.ProfessionGroupId, cancellationToken))
        {
            throw new NotFoundException(nameof(ProfessionGroups), request.ProfessionGroupId);
        }

        if (await context.Profession.AnyAsync(p => p.KzisCode == request.KzisCode, cancellationToken))
        {
            throw new ConflictException($"Profession with KzisCode {request.KzisCode} already exists.");
        }

        var profession = mapper.Map<Professions>(request);

        var (userId, username) = currentUserService.RequireAuditUser();
        var now = timeProvider.GetUtcNow();
        profession.CreatedAt = now;
        profession.CreatedById = userId;
        profession.CreatedByName = username;

        context.Profession.Add(profession);
        await context.SaveChangesAsync(cancellationToken);

        await context.Entry(profession).Reference(p => p.ProfessionGroup).LoadAsync(cancellationToken);

        return mapper.Map<ProfessionResponse>(profession);
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

        if (request.KzisCode is not null && request.KzisCode != profession.KzisCode)
        {
            bool duplicate = await context.Profession
                .AnyAsync(p => p.KzisCode == request.KzisCode && p.Id != id, cancellationToken);

            if (duplicate)
            {
                throw new ConflictException($"Profession with KzisCode {request.KzisCode} already exists.");
            }
        }

        if (request.ProfessionGroupId is not null && request.ProfessionGroupId != profession.ProfessionGroupId)
        {
            if (!await context.ProfessionGroup.AnyAsync(pg => pg.Id == request.ProfessionGroupId, cancellationToken))
            {
                throw new NotFoundException(nameof(ProfessionGroups), request.ProfessionGroupId.Value);
            }
        }

        mapper.Map(request, profession);

        var (userId, username) = currentUserService.RequireAuditUser();
        profession.UpdatedAt = timeProvider.GetUtcNow();
        profession.UpdatedById = userId;
        profession.UpdatedByName = username;

        await context.SaveChangesAsync(cancellationToken);

        await context.Entry(profession).Reference(p => p.ProfessionGroup).LoadAsync(cancellationToken);

        return mapper.Map<ProfessionResponse>(profession);
    }

    public async Task<ProfessionDetailResponse?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await context.Profession
            .Where(p => p.Id == id)
            .ProjectTo<ProfessionDetailResponse>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ProfessionResponse>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await context.Profession
            .OrderBy(p => p.KzisName)
            .ProjectTo<ProfessionResponse>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}
