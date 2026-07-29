// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Text;

namespace LaborStats.Application.Imports;

public sealed class ImportValidationException(IReadOnlyList<ImportValidationError> errors)
    : Exception("Import validation failed.")
{
    public IReadOnlyList<ImportValidationError> Errors { get; } = errors;
}
