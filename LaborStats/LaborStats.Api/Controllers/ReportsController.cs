using LaborStats.Application.Reports.Dtos;
using LaborStats.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LaborStats.Api.Controllers;
/// <summary>
/// Controller responsible for reports and statistical data aggregations.
/// </summary>
/// <param name="reportService">The reporting service.</param>
[ApiController]
[Route("api/reports")]
[Authorize]
public class ReportsController(IReportService reportService) : ControllerBase
{
    /// <summary>
    /// Retrieves aggregated statistical data based on the specified grouping and filters.
    /// </summary>
    [HttpGet("aggregated")]
    public async Task<ActionResult<IReadOnlyList<AggregatedReportItemResponse>>> GetAggregated(
        [FromQuery] ReportQueryParametersDto parameters,
        CancellationToken cancellationToken)
    {
        var result = await reportService.GetAggregatedDataAsync(parameters, cancellationToken);
        return Ok(result);
    }
}