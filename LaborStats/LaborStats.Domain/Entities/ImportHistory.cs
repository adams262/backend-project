using System;
using System.Collections.Generic;
using System.Text;

namespace LaborStats.Domain.Entities
{
    public class ImportHistory
    {
        public Guid Id { get; set; }
        public string FileName { get; set; } = null!;
        public string Voivodeship { get; set; } = null!;
        public string Period { get; set; } = null!;
        public DateTime ImportEndDate { get; set; }
        public int ProcessedRecordsCount { get; set; }
        public string CreatedBy { get; set; } = null!;
    }
}
