// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Security.Claims;
using FluentValidation;
using LaborStats.Application.Abstractions;
using LaborStats.Application.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LaborStats.Api.Controllers;

[ApiController]
[Route("api/users")]
public sealed class UsersController(
    IUserService userService,
    IValidator<CreateUserRequest> createUserValidator,
    IValidator<ChangeOwnPasswordRequest> changeOwnPasswordValidator,
    IValidator<SetUserPasswordRequest> setUserPasswordValidator) : ControllerBase
{
    /// <summary>
    /// Creates a new user. Requires administrator privileges.
    /// </summary>
    /// <param name="request">The new user's data, including a plain-text password (it will be hashed).</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <response code="201">The user was created.</response>
    /// <response code="400">The request is invalid.</response>
    /// <response code="401">Not authenticated.</response>
    /// <response code="403">Administrator privileges required.</response>
    /// <response code="409">A user with this name already exists.</response>

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<UserResponse>> Create(
        CreateUserRequest request,
        CancellationToken cancellationToken)
    {
        await createUserValidator.ValidateAndThrowAsync(request, cancellationToken);

        var result = await userService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Returns the list of all users. Requires administrator privileges.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <response code="200">The list of users.</response>
    /// <response code="401">Not authenticated.</response>
    /// <response code="403">Administrator privileges required.</response>

    [Authorize(Roles = "Admin")]
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<UserResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<IReadOnlyList<UserResponse>>> GetAll(
        CancellationToken cancellationToken) =>
        Ok(await userService.GetAllAsync(cancellationToken));

    /// <summary>
    /// Returns the details of a single user. Requires administrator privileges.
    /// </summary>
    /// <param name="id">The user's identifier.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <response code="200">The user's details.</response>
    /// <response code="401">Not authenticated.</response>
    /// <response code="403">Administrator privileges required.</response>
    /// <response code="404">The user does not exist.</response>

    [Authorize(Roles = "Admin")]
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(UserDetailResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserDetailResponse>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await userService.GetByIdAsync(id, cancellationToken);

        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    /// <summary>
    /// Assigns a new role to the specified user. Requires administrator privileges.
    /// </summary>
    /// <param name="id">The user's identifier.</param>
    /// <param name="request">The identifier of the new role.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <response code="204">The role was assigned.</response>
    /// <response code="401">Not authenticated.</response>
    /// <response code="403">Administrator privileges required.</response>
    /// <response code="404">The user does not exist.</response>

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:guid}/role")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AssignRole(
    Guid id,
    AssignRoleRequest request,
    CancellationToken cancellationToken)
    {
        await userService.AssignRoleAsync(id, request, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Changes the password of the currently authenticated user. Requires the current password.
    /// </summary>
    /// <param name="request">Current and new password.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <response code="204">The password was changed.</response>
    /// <response code="400">The request is invalid.</response>
    /// <response code="401">Not authenticated.</response>
    /// <response code="409">The current password is incorrect.</response>
    [Authorize]
    [HttpPut("me/password")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> ChangeOwnPassword(
        ChangeOwnPasswordRequest request,
        CancellationToken cancellationToken)
    {
        await changeOwnPasswordValidator.ValidateAndThrowAsync(request, cancellationToken);

        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userId = Guid.Parse(userIdClaim!);

        await userService.ChangeOwnPasswordAsync(userId, request, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Sets a new password for the specified user. Requires administrator privileges.
    /// </summary>
    /// <param name="id">The user's identifier.</param>
    /// <param name="request">The new password.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <response code="204">The password was set.</response>
    /// <response code="400">The request is invalid.</response>
    /// <response code="401">Not authenticated.</response>
    /// <response code="403">Administrator privileges required.</response>
    /// <response code="404">The user does not exist.</response>
    
    [Authorize(Roles = "Admin")]
    [HttpPut("{id:guid}/password")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SetPassword(
        Guid id,
        SetUserPasswordRequest request,
        CancellationToken cancellationToken)
    {
        await setUserPasswordValidator.ValidateAndThrowAsync(request, cancellationToken);

        await userService.SetPasswordAsync(id, request, cancellationToken);
        return NoContent();
    }
}

