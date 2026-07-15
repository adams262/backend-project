// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using LaborStats.Application.Abstractions;
using LaborStats.Application.Auth;
using Microsoft.AspNetCore.Mvc;

namespace LaborStats.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public sealed class AuthController(IAuthService authService) : ControllerBase
    {
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login(
            LoginRequest request,
            CancellationToken cancellationToken)
        {
            var result = await authService.LoginAsync(request, cancellationToken);
            return result is null ? Unauthorized() : Ok(result);
        }
    }
}
