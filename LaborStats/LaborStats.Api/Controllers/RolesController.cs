// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using FluentValidation;
using LaborStats.Application.Abstractions;
using LaborStats.Application.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LaborStats.Api.Controllers;

/// <summary>
/// Manages application roles.
/// </summary>
[ApiController]
[Route("api/roles")]
[Authorize(Roles = "Admin")]
public sealed class RolesController(IRoleService roleService, IValidator<CreateRoleRequest> createRoleValidator) : ControllerBase
{
    /// <summary>
    /// Creates a new role in the system. Requires administrator privileges.
    /// </summary>
    /// <param name="request">The name of the new role.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <response code="201">The role was created.</response>
    /// <response code="400">The request is invalid.</response>
    /// <response code="401">Not authenticated.</response>
    /// <response code="403">Administrator privileges required.</response>
    /// <response code="409">A role with this name already exists.</response>

    [HttpPost]
    [ProducesResponseType(typeof(RoleResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<RoleResponse>> Create(
        CreateRoleRequest request,
        CancellationToken cancellationToken)
    {
        await createRoleValidator.ValidateAndThrowAsync(request, cancellationToken);

        var result = await roleService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Returns the list of all roles available in the system. Requires administrator privileges.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <response code="200">The list of roles.</response>
    /// <response code="401">Not authenticated.</response>
    /// <response code="403">Administrator privileges required.</response>

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<RoleResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<IReadOnlyList<RoleResponse>>> GetAll(
        CancellationToken cancellationToken) =>
        Ok(await roleService.GetAllAsync(cancellationToken));

    /// <summary>
    /// Returns the details of a single role. Requires administrator privileges.
    /// </summary>
    /// <param name="id">The role's identifier.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <response code="200">The role's details.</response>
    /// <response code="401">Not authenticated.</response>
    /// <response code="403">Administrator privileges required.</response>
    /// <response code="404">The role does not exist.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(RoleResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RoleResponse>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await roleService.GetByIdAsync(id, cancellationToken);

        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }
}

