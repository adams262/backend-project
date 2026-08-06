namespace LaborStats.Application.Reports.Dtos;

public class ReportQueryParametersDto
{
    public string? GroupBy { get; set; } 
    public string? Voivodeship { get; set; }
    public string? CountyId { get; set; }
    public string? Period { get; set; }
    public string? DataType { get; set; }
}