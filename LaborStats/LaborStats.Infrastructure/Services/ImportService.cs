using LaborStats.Application.Abstractions;
using LaborStats.Application.Imports;
using LaborStats.Application.Imports.Dtos;

namespace LaborStats.Infrastructure.Services;

public class ImportService(IImportValidator importValidator) : IImportService
{
    public async Task ProcessImportAsync(
        ImportRequestDto request,
        string username,
        CancellationToken cancellationToken)
    {
        var validationResult = await importValidator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            throw new ImportValidationException(validationResult.Errors);
        }

        
    }
}
