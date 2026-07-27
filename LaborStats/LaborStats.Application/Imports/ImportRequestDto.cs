using Microsoft.AspNetCore.Http;
using LaborStats.Domain.Entities;

namespace LaborStats.Application.Imports.Dtos;

public class ImportRequestDto
{
    public string Voivodeship { get; set; } = string.Empty;
    public int Year { get; set; }                  
    public PeriodType Period { get; set; }
    public string DataType { get; set; } = string.Empty;
    public IFormFile File { get; set; } = null!;
}
