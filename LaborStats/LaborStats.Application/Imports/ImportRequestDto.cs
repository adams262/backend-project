using Microsoft.AspNetCore.Http;
using LaborStats.Domain.Entities;

namespace LaborStats.Application.Imports.Dtos;

public enum ImportDataType
{
    Employed = 1,
    NewlyHired = 2
}
public class ImportRequestDto
{
    public string Voivodeship { get; set; } = string.Empty;
    public int Year { get; set; }                  
    public PeriodType Period { get; set; }
    public ImportDataType DataType { get; set; }
    public IFormFile File { get; set; } = null!;
}
