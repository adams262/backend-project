namespace LaborStats.Application.Reports.Dtos;

public class AggregatedReportItemResponse
    {
        public string? Voivodeship { get; set; }
        public string? County { get; set; }
        public string? OccupationCode { get; set; }
        public string? Period { get; set; }
        public string? DataType { get; set; }

        public int? TotalContractsCount { get; set; }
        public int? LongTermContractsCount { get; set; }
        public int? NewlyRegisteredCount { get; set; }
    }