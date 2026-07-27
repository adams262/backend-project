using Microsoft.AspNetCore.Http;

namespace LaborStats.Application.Imports.Dtos;

public class ImportRequestDto
{
    public string Voivodeship { get; set; } = string.Empty;
    public string Period { get; set; } = string.Empty;
    public string DataType { get; set; } = string.Empty;
    public IFormFile File { get; set; } = null!;
}