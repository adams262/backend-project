using LaborStats.Application.Abstractions;
using LaborStats.Application.Imports;
using LaborStats.Domain.Entities;
using LaborStats.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LaborStats.Infrastructure.Services;

public sealed class ImportQueryService(LaborStatsDbContext context) : IImportQueryService
{
    public async Task<IReadOnlyList<ImportListItemResponse>>
        GetAllAsync(
            CancellationToken cancellationToken = default)
    {
        List<ImportHistory> imports =
            await context.ImportHistories
                .AsNoTracking()
                .OrderByDescending(importHistory =>
                    importHistory.ImportEndDate)
                .ToListAsync(cancellationToken);

        return
        [
            .. imports.Select(importHistory =>
                importHistory.ToListItemResponse())
        ];
    }

    public async Task<ImportDetailsResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        ImportHistory? importHistory =
            await context.ImportHistories
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    importHistory =>
                        importHistory.Id == id,
                    cancellationToken);

        return importHistory?.ToDetailsResponse();
    }

    public async Task<IReadOnlyList<LaborStatRecordResponse>>
        GetRecordsAsync(
            Guid importId,
            CancellationToken cancellationToken = default)
    {
        List<LaborStatRecord> records =
            await context.LaborStatRecords
                .AsNoTracking()
                .Where(record =>
                    record.ImportHistoryId == importId)
                .OrderBy(record => record.County)
                .ThenBy(record => record.OccupationCode)
                .ToListAsync(cancellationToken);

        return
        [
            .. records.Select(record =>
                record.ToResponse())
        ];
    }
}
