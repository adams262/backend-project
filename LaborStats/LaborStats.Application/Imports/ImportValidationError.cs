// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Text;

namespace LaborStats.Application.Imports;

public sealed record ImportValidationError(
    int? RowNumber,
    string? ColumnName,
    string? SourceValue,
    string Description);

public sealed record ImportValidationResult(
    bool IsValid,
    IReadOnlyList<ImportValidationError> Errors);
