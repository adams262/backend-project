using ClosedXML.Excel;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LaborStats.Application.Abstractions;
using LaborStats.Application.Imports.Dtos;

namespace LaborStats.Application.Imports;

public class ImportValidationService : IImportValidator
{
    private const string PowiatColumn = "POWIAT";
    private const string OccupationCodeColumn = "KOD ZAWODU UBEZPIECZONEGO";
    private const string AllContractsColumn = "LICZBA UBEZPIECZONYCH WSZYSTKICH UMOW";
    private const string Over2YearsColumn = "LICZBA UBEZPIECZONYCH UMOW POWYZEJ 2 LAT";
    private const string NewlyRegisteredColumn = "NOWO ZAREJESTROWANY";

    public async Task<ImportValidationResult> ValidateAsync(
        ImportRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var errors = new List<ImportValidationError>();

        if (request.File == null || request.File.Length == 0)
        {
            errors.Add(new ImportValidationError(
                RowNumber: null,
                ColumnName: "PLIK",
                SourceValue: null,
                Description: "brak wymaganej wartości (plik nie został dołączony)"
            ));

            return new ImportValidationResult(false, errors);
        }

        using var stream = request.File.OpenReadStream();
        using var workbook = new XLWorkbook(stream);
        var worksheet = workbook.Worksheet(1);

        var headerRow = worksheet.Row(1);
        var headerMap = new Dictionary<string, int>();

        for (int col = 1; col <= headerRow.LastCellUsed().Address.ColumnNumber; col++)
        {
            string headerText = headerRow.Cell(col).GetFormattedString().Trim().ToUpper();
            headerMap[headerText] = col;
        }

        int powiatCol = FindColumnIndexSafe(headerMap, PowiatColumn);
        int professionCol = FindColumnIndexSafe(headerMap, OccupationCodeColumn);

        int allContractsCol = 0;
        int over2YearsCol = 0;
        int newlyRegisteredCol = 0;

        if (request.DataType == ImportDataType.Employed)
        {
            allContractsCol = FindColumnIndexSafe(headerMap, AllContractsColumn);
            over2YearsCol = FindColumnIndexSafe(headerMap, Over2YearsColumn);
        }
        else if (request.DataType == ImportDataType.NewlyHired)
        {
            newlyRegisteredCol = FindColumnIndexSafe(headerMap, NewlyRegisteredColumn);
        }

        int rowNumber = 1;

        foreach (var row in worksheet.RowsUsed().Skip(1))
        {
            cancellationToken.ThrowIfCancellationRequested();
            rowNumber++;

            var county = powiatCol > 0 ? row.Cell(powiatCol).GetFormattedString()?.Trim() : string.Empty;
            if (string.IsNullOrEmpty(county))
            {
                errors.Add(new ImportValidationError(
                    RowNumber: rowNumber,
                    ColumnName: PowiatColumn,
                    SourceValue: county,
                    Description: "brak wymaganej wartości"
                ));
            }

            var professionCode = professionCol > 0 ? row.Cell(professionCol).GetFormattedString()?.Trim() : string.Empty;
            if (string.IsNullOrEmpty(professionCode) || !int.TryParse(professionCode, out _))
            {
                errors.Add(new ImportValidationError(
                    RowNumber: rowNumber,
                    ColumnName: OccupationCodeColumn,
                    SourceValue: professionCode,
                    Description: "niepoprawna liczba"
                ));
            }

            if (request.DataType == ImportDataType.NewlyHired && newlyRegisteredCol > 0)
            {
                var countValue = row.Cell(newlyRegisteredCol).GetFormattedString()?.Trim();
                if (string.IsNullOrEmpty(countValue) || !int.TryParse(countValue, out var val) || val < 0)
                {
                    errors.Add(new ImportValidationError(
                        RowNumber: rowNumber,
                        ColumnName: NewlyRegisteredColumn,
                        SourceValue: countValue,
                        Description: "niepoprawna liczba"
                    ));
                }
            }
            else if (request.DataType == ImportDataType.Employed)
            {
                if (allContractsCol > 0)
                {
                    var totalCount = row.Cell(allContractsCol).GetFormattedString()?.Trim();
                    if (string.IsNullOrEmpty(totalCount) || !int.TryParse(totalCount, out var val1) || val1 < 0)
                    {
                        errors.Add(new ImportValidationError(
                            RowNumber: rowNumber,
                            ColumnName: AllContractsColumn,
                            SourceValue: totalCount,
                            Description: "niepoprawna liczba"
                        ));
                    }
                }

                if (over2YearsCol > 0)
                {
                    var overTwoYearsCount = row.Cell(over2YearsCol).GetFormattedString()?.Trim();
                    if (string.IsNullOrEmpty(overTwoYearsCount) || !int.TryParse(overTwoYearsCount, out var val2) || val2 < 0)
                    {
                        errors.Add(new ImportValidationError(
                            RowNumber: rowNumber,
                            ColumnName: Over2YearsColumn,
                            SourceValue: overTwoYearsCount,
                            Description: "niepoprawna liczba"
                        ));
                    }
                }
            }
        }

        return new ImportValidationResult(
            IsValid: errors.Count == 0,
            Errors: errors
        );
    }

    private static int FindColumnIndexSafe(Dictionary<string, int> headerMap, string keyword)
    {
        var match = headerMap.FirstOrDefault(h => h.Key.Contains(keyword, StringComparison.OrdinalIgnoreCase));
        return match.Value; 
    }
}