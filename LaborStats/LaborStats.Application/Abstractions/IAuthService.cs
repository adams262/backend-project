// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Text;
using LaborStats.Application.Auth;

namespace LaborStats.Application.Abstractions;

public interface IAuthService
{
    Task<AuthResponse?> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
    Task<RefreshTokenResponse?> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken = default);
    Task<bool> RevokeTokenAsync(RevokeTokenRequest request, CancellationToken cancellationToken = default);
}
