using LaborStats.Application.Abstractions;
using LaborStats.Application.Imports.Dtos;

namespace LaborStats.Infrastructure.Services;

public class ImportService : IImportService
{
    public async Task ProcessImportAsync(ImportRequestDto request, string username, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
    }
}