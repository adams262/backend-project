using System;
using System.Collections.Generic;
using System.Text;

namespace LaborStats.Domain.Entities
{
    public class LaborStatRecord
    {
        public Guid Id { get; set; }
        public Guid ImportHistoryId { get; set; }
        public ImportHistory ImportHistory { get; set; } = null!;
        public string Voivodeship { get; set; } = null!;
        public string Powiat { get; set; } = null!;
        public string OccupationCode { get; set; } = null!;
        public string InsuranceTitleCode { get; set; } = null!;
        public int? TotalContractsCount { get; set; }
        public int? LongTermContractsCount { get; set; }
        public int? NewlyRegisteredCount { get; set; }
        public string DataType { get; set; } = null!;
    }
}
