// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace LaborStats.Application.Users;

public sealed class SetUserPasswordRequestValidator : AbstractValidator<SetUserPasswordRequest>
{
    public SetUserPasswordRequestValidator()
    {
        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .WithMessage("New password is required.")
            .MinimumLength(8)
            .WithMessage("New password must be at least 8 characters long.");
    }
}
