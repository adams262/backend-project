using LaborStats.Application.Imports.Dtos;

namespace LaborStats.Application.Abstractions;

public interface IDataConversionService
{
    Task<List<ConvertedImportRowDto>> ConvertAsync(Stream fileStream, string voivodeshipName, ImportDataType dataType, CancellationToken cancellationToken);
}
