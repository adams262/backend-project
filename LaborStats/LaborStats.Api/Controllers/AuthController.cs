// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using LaborStats.Application.Abstractions;
using LaborStats.Application.Auth;
using Microsoft.AspNetCore.Mvc;

namespace LaborStats.Api.Controllers;

/// <summary>
/// Handles user authentication and token lifecycle.
/// </summary>
[ApiController]
[Route("api/auth")]
public sealed class AuthController(IAuthService authService) : ControllerBase
{
    /// <summary>
    /// Logs in a user and returns a JWT token along with its expiration time.
    /// </summary>
    /// <param name="request">The user's login and password.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <response code="200">Returns the JWT token after a successful login.</response>
    /// <response code="401">Invalid login or password.</response>
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<LoginResponse>> Login(
        LoginRequest request,
        CancellationToken cancellationToken)
    {
        var result = await authService.LoginAsync(request, cancellationToken);

        if (result is null)
        {
            return Unauthorized();
        }

        return Ok(result);
    }
}

