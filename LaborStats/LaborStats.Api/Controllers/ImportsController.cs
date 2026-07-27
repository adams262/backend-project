using System.Security.Claims;
using LaborStats.Application.Abstractions;
using LaborStats.Application.Imports.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LaborStats.Api.Controllers;

/// <summary>
/// Controller responsible for handling import operations.
/// </summary>
[ApiController]
[Route("api/imports")]
[Authorize(Roles = "Admin")]
public sealed class ImportsController(IImportService importService) : ControllerBase
{
    /// <summary>
    /// Imports labor statistics file for processing. Requires administrator privileges.
    /// </summary>
    /// <param name="request">The file upload request payload.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <response code="200">The file was successfully queued for processing.</response>
    /// <response code="400">The uploaded file is empty or invalid.</response>
    /// <response code="401">Not authenticated.</response>
    /// <response code="403">Administrator privileges required.</response>
    [HttpPost]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Import([FromForm] ImportRequestDto request, CancellationToken cancellationToken)
    {
        if (request.File is null || request.File.Length == 0)
        {
            return BadRequest("Source file needed!");
        }

        var username = User.FindFirstValue(ClaimTypes.Name) 
                    ?? User.FindFirstValue(ClaimTypes.Email) 
                    ?? "Admin";

        await importService.ProcessImportAsync(request, username, cancellationToken);

        return Ok(new { Message = "Import został pomyślnie przekazany do przetworzenia." });
    }
}
