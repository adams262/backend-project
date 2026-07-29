using FluentValidation;
using LaborStats.Application.Abstractions;
using LaborStats.Application.Professions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LaborStats.Api.Controllers;

/// <summary>
/// Manages professions.
/// </summary>
[ApiController]
[Route("api/professions")]
[Authorize(Roles = "Admin")]
public sealed class ProfessionsController(
    IProfessionService professionService,
    IValidator<CreateProfessionRequest> createValidator,
    IValidator<UpdateProfessionRequest> updateValidator) : ControllerBase
{
    /// <summary>
    /// Adds a new profession to a profession group. Requires administrator privileges.
    /// </summary>
    /// <param name="request">The new profession's data, including the target group identifier.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <response code="201">The profession was created.</response>
    /// <response code="400">The request is invalid.</response>
    /// <response code="401">Not authenticated.</response>
    /// <response code="403">Administrator privileges required.</response>
    /// <response code="404">The referenced profession group does not exist.</response>
    /// <response code="409">A profession with this KzisCode already exists.</response>
    [HttpPost]
    [ProducesResponseType(typeof(ProfessionResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ProfessionResponse>> Create(
        CreateProfessionRequest request,
        CancellationToken cancellationToken)
    {
        await createValidator.ValidateAndThrowAsync(request, cancellationToken);

        var result = await professionService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Updates an existing profession. Requires administrator privileges.
    /// Only the supplied fields are updated.
    /// </summary>
    /// <param name="id">The profession's identifier.</param>
    /// <param name="request">The fields to update.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <response code="200">The updated profession.</response>
    /// <response code="400">The request is invalid.</response>
    /// <response code="401">Not authenticated.</response>
    /// <response code="403">Administrator privileges required.</response>
    /// <response code="404">The profession or referenced profession group does not exist.</response>
    /// <response code="409">A profession with this KzisCode already exists.</response>
    [HttpPatch("{id:guid}")]
    [ProducesResponseType(typeof(ProfessionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ProfessionResponse>> Update(
        Guid id,
        UpdateProfessionRequest request,
        CancellationToken cancellationToken)
    {
        await updateValidator.ValidateAndThrowAsync(request, cancellationToken);

        var result = await professionService.UpdateAsync(id, request, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Returns the details of a single profession. Requires administrator privileges.
    /// </summary>
    /// <param name="id">The profession's identifier.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <response code="200">The profession's details.</response>
    /// <response code="401">Not authenticated.</response>
    /// <response code="403">Administrator privileges required.</response>
    /// <response code="404">The profession does not exist.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ProfessionDetailResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProfessionDetailResponse>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await professionService.GetByIdAsync(id, cancellationToken);

        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    /// <summary>
    /// Returns the list of all professions. Requires administrator privileges.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <response code="200">The list of professions.</response>
    /// <response code="401">Not authenticated.</response>
    /// <response code="403">Administrator privileges required.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<ProfessionResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<IReadOnlyList<ProfessionResponse>>> GetAll(
        CancellationToken cancellationToken) =>
        Ok(await professionService.GetAllAsync(cancellationToken));
}
