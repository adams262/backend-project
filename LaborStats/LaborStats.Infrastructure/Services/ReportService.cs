using LaborStats.Application.Reports.Dtos;
using LaborStats.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LaborStats.Infrastructure.Services;

public interface IReportService
{
    Task<IReadOnlyList<AggregatedReportItemResponse>> GetAggregatedDataAsync(
        ReportQueryParametersDto parameters,
        CancellationToken cancellationToken = default);
}

public class ReportService(LaborStatsDbContext dbContext) : IReportService
{
    public async Task<IReadOnlyList<AggregatedReportItemResponse>> GetAggregatedDataAsync(
        ReportQueryParametersDto parameters,
        CancellationToken cancellationToken = default)
    {
        var query = dbContext.LaborStatRecords.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(parameters.Voivodeship))
        {
            query = query.Where(r => r.Voivodeship == parameters.Voivodeship);
        }

        if (!string.IsNullOrWhiteSpace(parameters.CountyId))
        {
            query = query.Where(r => r.County == parameters.CountyId);
        }

        if (!string.IsNullOrWhiteSpace(parameters.DataType))
        {
            query = query.Where(r => r.DataType == parameters.DataType);
        }

        var records = await query.ToListAsync(cancellationToken);

        string? groupBy = parameters.GroupBy?.ToLower();

        var groupedResult = records
            .GroupBy(r => new
            {
                Voivodeship = groupBy == "voivodeship" ? r.Voivodeship : null,
                County = groupBy == "county" ? r.County : null,
                OccupationCode = groupBy == "profession" ? r.OccupationCode : null,
                DataType = r.DataType
            })
            .Select(g => new AggregatedReportItemResponse
            {
                Voivodeship = g.Key.Voivodeship,
                County = g.Key.County,
                OccupationCode = g.Key.OccupationCode,
                DataType = g.Key.DataType,
                TotalContractsCount = g.Sum(x => x.TotalContractsCount),
                LongTermContractsCount = g.Sum(x => x.LongTermContractsCount),
                NewlyRegisteredCount = g.Sum(x => x.NewlyRegisteredCount)
            })
            .ToList();

        return groupedResult;
    }
}