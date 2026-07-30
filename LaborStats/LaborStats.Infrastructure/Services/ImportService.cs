using LaborStats.Application.Abstractions;
using LaborStats.Application.Imports;
using LaborStats.Application.Imports.Dtos;

namespace LaborStats.Infrastructure.Services;

public class ImportService(IImportValidator importValidator, IImportIdempotencyChecker idempotencyChecker) : IImportService
{
    public async Task ProcessImportAsync(
        ImportRequestDto request,
        string username,
        CancellationToken cancellationToken)
    {

        await idempotencyChecker.EnsureNotAlreadyImportedAsync(
            request.Voivodeship, request.Year, request.Period, request.DataType, cancellationToken);

        var validationResult = await importValidator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            throw new ImportValidationException(validationResult.Errors);
        }

        
    }
}
