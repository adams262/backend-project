
using System;
using System.Collections.Generic;
using System.Text;
using LaborStats.Application.Imports.Dtos;
using LaborStats.Domain.Entities;

namespace LaborStats.Application.Abstractions;

public interface IImportIdempotencyChecker
{
    Task EnsureNotAlreadyImportedAsync(
        string voivodeship,
        int year,
        PeriodType period,
        ImportDataType dataType,
        CancellationToken cancellationToken = default);
}
