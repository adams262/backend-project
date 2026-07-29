using FluentValidation;
using LaborStats.Application.Abstractions;
using LaborStats.Application.ProfessionGroups;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LaborStats.Api.Controllers;

/// <summary>
/// Manages profession groups.
/// </summary>
[ApiController]
[Route("api/profession-groups")]
[Authorize(Roles = "Admin")]
public sealed class ProfessionGroupsController(
    IProfessionGroupService professionGroupService,
    IValidator<CreateProfessionGroupRequest> createValidator,
    IValidator<UpdateProfessionGroupRequest> updateValidator) : ControllerBase
{
    /// <summary>
    /// Creates a new profession group. Requires administrator privileges.
    /// </summary>
    /// <param name="request">The new profession group's data.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <response code="201">The profession group was created.</response>
    /// <response code="400">The request is invalid.</response>
    /// <response code="401">Not authenticated.</response>
    /// <response code="403">Administrator privileges required.</response>
    /// <response code="409">A profession group with this name already exists.</response>
    [HttpPost]
    [ProducesResponseType(typeof(ProfessionGroupResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ProfessionGroupResponse>> Create(
        CreateProfessionGroupRequest request,
        CancellationToken cancellationToken)
    {
        await createValidator.ValidateAndThrowAsync(request, cancellationToken);

        var result = await professionGroupService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Updates an existing profession group. Requires administrator privileges.
    /// Only the supplied fields are updated.
    /// </summary>
    /// <param name="id">The profession group's identifier.</param>
    /// <param name="request">The fields to update.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <response code="200">The updated profession group.</response>
    /// <response code="400">The request is invalid.</response>
    /// <response code="401">Not authenticated.</response>
    /// <response code="403">Administrator privileges required.</response>
    /// <response code="404">The profession group does not exist.</response>
    /// <response code="409">A profession group with this name already exists.</response>
    [HttpPatch("{id:guid}")]
    [ProducesResponseType(typeof(ProfessionGroupResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ProfessionGroupResponse>> Update(
        Guid id,
        UpdateProfessionGroupRequest request,
        CancellationToken cancellationToken)
    {
        await updateValidator.ValidateAndThrowAsync(request, cancellationToken);

        var result = await professionGroupService.UpdateAsync(id, request, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Returns the details of a single profession group, including its professions.
    /// Requires administrator privileges.
    /// </summary>
    /// <param name="id">The profession group's identifier.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <response code="200">The profession group's details.</response>
    /// <response code="401">Not authenticated.</response>
    /// <response code="403">Administrator privileges required.</response>
    /// <response code="404">The profession group does not exist.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ProfessionGroupDetailResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProfessionGroupDetailResponse>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await professionGroupService.GetByIdAsync(id, cancellationToken);

        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    /// <summary>
    /// Returns the list of all profession groups. Requires administrator privileges.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <response code="200">The list of profession groups.</response>
    /// <response code="401">Not authenticated.</response>
    /// <response code="403">Administrator privileges required.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<ProfessionGroupResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<IReadOnlyList<ProfessionGroupResponse>>> GetAll(
        CancellationToken cancellationToken) =>
        Ok(await professionGroupService.GetAllAsync(cancellationToken));
}
