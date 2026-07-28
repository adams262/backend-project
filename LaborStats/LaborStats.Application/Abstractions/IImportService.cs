using LaborStats.Application.Imports.Dtos;

namespace LaborStats.Application.Abstractions;

public interface IImportService
{
    Task ProcessImportAsync(ImportRequestDto dto, 
            string createdBy, CancellationToken 
            cancellationToken = default);
}