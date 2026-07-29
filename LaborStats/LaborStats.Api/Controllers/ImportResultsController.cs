// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using LaborStats.Application.Abstractions;
using LaborStats.Application.Imports;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LaborStats.Api.Controllers;

/// <summary>
/// Provides access to completed imports and processed records.
/// </summary>
[ApiController]
[Route("api/imports")]
[Authorize(Roles = "Admin")]
public sealed class ImportResultsController(
    IImportQueryService importQueryService)
    : ControllerBase
{
    /// <summary>
    /// Returns the list of completed data imports. Requires administrator privileges.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <response code="200">
    /// The list of imports was successfully retrieved. Returns an empty collection
    /// when no imports exist.
    /// </response>
    /// <response code="401">Not authenticated.</response>
    /// <response code="403">Administrator privileges required.</response>
    [HttpGet]
    [ProducesResponseType(
        typeof(IReadOnlyList<ImportListItemResponse>),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<
        ActionResult<IReadOnlyList<ImportListItemResponse>>>
        GetAll(CancellationToken cancellationToken)
    {
        return Ok(
            await importQueryService.GetAllAsync(
                cancellationToken));
    }

    /// <summary>
    /// Returns detailed information about a selected data import.
    /// Requires administrator privileges.
    /// </summary>
    /// <param name="id">The unique identifier of the import.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <response code="200">The import details were successfully retrieved.</response>
    /// <response code="401">Not authenticated.</response>
    /// <response code="403">Administrator privileges required.</response>
    /// <response code="404">An import with the specified identifier was not found.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(
        typeof(ImportDetailsResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ImportDetailsResponse>>
        GetById(
            Guid id,
            CancellationToken cancellationToken)
    {
        ImportDetailsResponse? import =
            await importQueryService.GetByIdAsync(
                id,
                cancellationToken);

        if (import is null)
        {
            return NotFound();
        }

        return Ok(import);
    }

    /// <summary>
    /// Returns the processed labor statistics records associated with a selected import.
    /// Requires administrator privileges.
    /// </summary>
    /// <param name="id">The unique identifier of the import.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <response code="200">
    /// The processed records were successfully retrieved. Returns an empty collection
    /// when no imports exist for the specified import identifier.
    /// </response>
    /// <response code="401">Not authenticated.</response>
    /// <response code="403">Administrator privileges required.</response>
    [HttpGet("{id:guid}/records")]
    [ProducesResponseType(
        typeof(IReadOnlyList<LaborStatRecordResponse>),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<
        ActionResult<IReadOnlyList<LaborStatRecordResponse>>>
        GetRecords(
            Guid id,
            CancellationToken cancellationToken)
    {
        return Ok(
            await importQueryService.GetRecordsAsync(
                id,
                cancellationToken));
    }
}
