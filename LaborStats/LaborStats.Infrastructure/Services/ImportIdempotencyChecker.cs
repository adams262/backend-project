
using System;
using System.Collections.Generic;
using System.Text;
using LaborStats.Application.Abstractions;
using LaborStats.Application.Imports.Dtos;
using LaborStats.Domain.Entities;
using LaborStats.Domain.Exceptions;
using LaborStats.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LaborStats.Infrastructure.Services;

public sealed class ImportIdempotencyChecker(LaborStatsDbContext context) : IImportIdempotencyChecker
{
    public async Task EnsureNotAlreadyImportedAsync(
        string voivodeship,
        int year,
        PeriodType period,
        ImportDataType dataType,
        CancellationToken cancellationToken = default)
    {
        bool alreadyImported = await context.LaborStatRecords
            .AsNoTracking()
            .AnyAsync(r =>
                r.DataType == dataType.ToString()
                && r.ImportHistory.Voivodeship == voivodeship
                && r.ImportHistory.Year == year
                && r.ImportHistory.Period == period,
                cancellationToken);

        if (alreadyImported)
        {
            throw new ImportAlreadyExistsException(
                $"Data for voivodeship '{voivodeship}', period '{period} {year}' and data type '{dataType}' has already been imported.");
        }
    }
}
