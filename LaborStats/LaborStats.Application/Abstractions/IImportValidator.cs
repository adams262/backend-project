// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Text;
using LaborStats.Application.Imports;
using LaborStats.Application.Imports.Dtos;

namespace LaborStats.Application.Abstractions;

public interface IImportValidator
{
    Task<ImportValidationResult> ValidateAsync(
        ImportRequestDto request,
        CancellationToken cancellationToken = default);
}
