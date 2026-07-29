namespace LaborStats.Application.Imports.Dtos;

public class ConvertedImportRowDto
{
    public string CountyId { get; set; } = string.Empty; 
    public Guid ProfessionId { get; set; }   
    public int Value { get; set; }           
    public string Period { get; set; } = string.Empty;     
    public string DataType { get; set; } = string.Empty;   
}