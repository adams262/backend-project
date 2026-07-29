using LaborStats.Application.Imports.Dtos;

namespace LaborStats.Application.Abstractions;

public interface IDataConversionService
{
    Task<List<ConvertedImportRowDto>> ConvertAsync(Stream fileStream, CancellationToken cancellationToken);
}