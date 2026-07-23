using System;
using System.Collections.Generic;
using System.Text;

namespace LaborStats.Domain.Entities
{
    public enum PeriodType : byte
    {
        FirstHalf = 1,
        SecondHalf = 2
    }
    public class ImportHistory
    {
        public Guid Id { get; set; }
        public string FileName { get; set; } = null!;
        public string Voivodeship { get; set; } = null!;
        public int Year { get; set; }
        public PeriodType Period { get; set; }
        public DateTime ImportEndDate { get; set; }
        public int ProcessedRecordsCount { get; set; }
        public string CreatedBy { get; set; } = null!;
    }
}
