using ExcelDataReader;
using LaborStats.Application.Abstractions;
using LaborStats.Application.Imports;
using LaborStats.Application.Imports.Dtos;
using LaborStats.Domain.Entities;
using LaborStats.Infrastructure.Data;
using LaborStats.Infrastructure.Helpers; 
using Microsoft.EntityFrameworkCore;

namespace LaborStats.Infrastructure.Services;

public sealed class ImportValidator : IImportValidator
{
    private const string VoivodeshipCol = "WOJEWODZTWO";
    private const string CountyCol = "POWIAT";
    private const string OccupationCodeCol = "KOD ZAWODU UBEZPIECZONEGO";
    private const string InsuranceTitleCol = "KOD TYTULU UBEZPIECZENIA";
    private const string EmployedTotalCol = "LICZBA UBEZPIECZONYCH WSZYSTKICH UMOW";
    private const string EmployedOver2YearsCol = "LICZBA UBEZPIECZONYCH UMOW POWYZEJ 2 LAT";
    private const string NewlyHiredCol = "LICZBA UBEZPIECZONYCH (UMOW) - NOWO ZAREJESTROWANYCH";
    private const string HeaderRowIdentifier = "WOJEW";

    private readonly LaborStatsDbContext _context;

    public ImportValidator(LaborStatsDbContext context)
    {
        _context = context;
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
    }

    public async Task<ImportValidationResult> ValidateAsync(
        ImportRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var errors = new List<ImportValidationError>();

        await ValidateRequestAsync(request, errors, cancellationToken);
        if (errors.Count > 0)
        {
            return new ImportValidationResult(false, errors);
        }

        bool isEmployed = request.DataType == ImportDataType.Employed;
        string[] requiredColumns = isEmployed
            ? [VoivodeshipCol, CountyCol, OccupationCodeCol, InsuranceTitleCol, EmployedTotalCol, EmployedOver2YearsCol]
            : [VoivodeshipCol, CountyCol, OccupationCodeCol, InsuranceTitleCol, NewlyHiredCol];

        using var stream = request.File.OpenReadStream();
        using var reader = ExcelReaderFactory.CreateReader(stream);

        var columnIndex = ResolveColumns(reader, requiredColumns, errors);
        if (errors.Count > 0)
        {
            return new ImportValidationResult(false, errors);
        }

        var (knownCounties, knownOccupationCodes) = await LoadDictionariesAsync(request, cancellationToken);

        ValidateRows(reader, columnIndex, isEmployed, request, knownCounties, knownOccupationCodes, errors);

        return new ImportValidationResult(errors.Count == 0, errors);
    }

    private async Task ValidateRequestAsync(
        ImportRequestDto request,
        List<ImportValidationError> errors,
        CancellationToken cancellationToken)
    {
        if (request.File == null || request.File.Length == 0)
        {
            errors.Add(new ImportValidationError(null, "File", null, "The import file is required and cannot be empty."));
            return;
        }

        bool voivodeshipExists = await _context.Voivodeships
            .AsNoTracking()
            .AnyAsync(v => v.Name.ToLower() == request.Voivodeship.Trim().ToLower(), cancellationToken);

        if (!voivodeshipExists)
        {
            errors.Add(new ImportValidationError(null, "Voivodeship", request.Voivodeship,
                $"The specified voivodeship '{request.Voivodeship}' does not exist in the system."));
        }

        if (request.Year < 2000 || request.Year > 2100)
        {
            errors.Add(new ImportValidationError(null, "Year", request.Year.ToString(),
                $"The year '{request.Year}' is invalid. Expected a year between 2000 and 2100."));
        }

        if (!Enum.IsDefined(typeof(PeriodType), request.Period))
        {
            errors.Add(new ImportValidationError(null, "Period", request.Period.ToString(),
                "The period is invalid. Expected FirstHalf (1) or SecondHalf (2)."));
        }

        if (!Enum.IsDefined(typeof(ImportDataType), request.DataType))
        {
            errors.Add(new ImportValidationError(null, "DataType", request.DataType.ToString(),
                "The data type is invalid. Expected Employed (1) or NewlyHired (2)."));
        }
    }

    private static Dictionary<string, int> ResolveColumns(
        IExcelDataReader reader,
        string[] requiredColumns,
        List<ImportValidationError> errors)
    {
        var columnIndex = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        bool headerFound = false;
        int rowNumber = 0;

        while (rowNumber < 5 && reader.Read())
        {
            rowNumber++;

            for (int i = 0; i < reader.FieldCount; i++)
            {
                string val = ExcelHelper.NormalizeHeader(reader.GetValue(i)?.ToString() ?? string.Empty);

                if (val.Contains(HeaderRowIdentifier, StringComparison.OrdinalIgnoreCase))
                {
                    for (int j = 0; j < reader.FieldCount; j++)
                    {
                        string header = ExcelHelper.NormalizeHeader(reader.GetValue(j)?.ToString() ?? string.Empty);

                        if (!string.IsNullOrEmpty(header))
                        {
                            columnIndex[header] = j;
                        }
                    }
                    headerFound = true;
                    break;
                }
            }

            if (headerFound) break;
        }

        if (!headerFound)
        {
            errors.Add(new ImportValidationError(null, null, null, $"The file does not contain a valid header row with the '{VoivodeshipCol}' column."));
            return columnIndex;
        }

        foreach (var required in requiredColumns)
        {
            bool found = columnIndex.Keys.Any(k => k.StartsWith(required, StringComparison.OrdinalIgnoreCase));
            if (!found)
            {
                errors.Add(new ImportValidationError(null, required, null, $"Required column '{required}' was not found in the file."));
            }
        }

        return columnIndex;
    }

    private async Task<(HashSet<string> Counties, HashSet<int> OccupationCodes)> LoadDictionariesAsync(
        ImportRequestDto request,
        CancellationToken cancellationToken)
    {
        var knownCounties = new HashSet<string>(
            await _context.Counties.AsNoTracking()
                .Where(c => c.Voivodeship.Name.ToLower() == request.Voivodeship.Trim().ToLower())
                .Select(c => c.Name)
                .ToListAsync(cancellationToken),
            StringComparer.OrdinalIgnoreCase);

        var knownOccupationCodes = new HashSet<int>(
            await _context.Profession.AsNoTracking()
                .Select(p => p.KzisCode)
                .ToListAsync(cancellationToken));

        return (knownCounties, knownOccupationCodes);
    }

    private static void ValidateRows(
        IExcelDataReader reader,
        Dictionary<string, int> columnIndex,
        bool isEmployed,
        ImportRequestDto request,
        HashSet<string> knownCounties,
        HashSet<int> knownOccupationCodes,
        List<ImportValidationError> errors)
    {
        var seenRecords = new HashSet<(string County, int Code, string Title)>();

        string numericColumn = isEmployed ? EmployedTotalCol : NewlyHiredCol;

        int voivodeshipIdx = ExcelHelper.FindColumn(columnIndex, VoivodeshipCol);
        int countyIdx = ExcelHelper.FindColumn(columnIndex, CountyCol);
        int occupationIdx = ExcelHelper.FindColumn(columnIndex, OccupationCodeCol);
        int insuranceTitleIdx = ExcelHelper.FindColumn(columnIndex, InsuranceTitleCol);
        int numericIdx = ExcelHelper.FindColumn(columnIndex, numericColumn);

        int processedRows = 0;
        int rowNumber = 0;

        while (reader.Read())
        {
            rowNumber++;

            string voivodeshipCell = ExcelHelper.GetString(reader, voivodeshipIdx);
            string cleanedVoivodeship = voivodeshipCell.Replace(",", "").Trim();

            if (string.IsNullOrWhiteSpace(cleanedVoivodeship) || cleanedVoivodeship == "-")
            {
                continue;
            }

            processedRows++;

            string county = ExcelHelper.GetString(reader, countyIdx);
            string occupationStr = ExcelHelper.GetString(reader, occupationIdx);
            string insuranceTitle = ExcelHelper.GetString(reader, insuranceTitleIdx);
            string numericValue = ExcelHelper.GetString(reader, numericIdx);

            if (string.IsNullOrWhiteSpace(county))
            {
                errors.Add(new ImportValidationError(rowNumber, CountyCol, null, "The county field is required."));
            }
            else if (!knownCounties.Contains(county))
            {
                errors.Add(new ImportValidationError(rowNumber, CountyCol, county,
                    $"County '{county}' does not belong to the specified voivodeship '{request.Voivodeship}' or does not exist."));
            }

            int occupationCode = 0;
            if (string.IsNullOrWhiteSpace(occupationStr))
            {
                errors.Add(new ImportValidationError(rowNumber, OccupationCodeCol, null, "Occupation code is required."));
            }
            else if (!int.TryParse(occupationStr, out occupationCode))
            {
                errors.Add(new ImportValidationError(rowNumber, OccupationCodeCol, occupationStr,
                    "Occupation code is not in a valid numeric format."));
            }
            else if (!knownOccupationCodes.Contains(occupationCode))
            {
                errors.Add(new ImportValidationError(rowNumber, OccupationCodeCol, occupationStr,
                    $"Occupation code '{occupationCode}' does not exist in the KZiS dictionary."));
            }

            if (string.IsNullOrWhiteSpace(numericValue))
            {
                errors.Add(new ImportValidationError(rowNumber, numericColumn, null, "Numeric value is required."));
            }
            else if (!int.TryParse(numericValue, out int parsedNum) || parsedNum < 0)
            {
                errors.Add(new ImportValidationError(rowNumber, numericColumn, numericValue,
                    "Numeric value has an invalid format or is negative."));
            }

            if (!string.IsNullOrWhiteSpace(county) && occupationCode != 0)
            {
                var recordKey = (county.ToLower(), occupationCode, insuranceTitle.ToLower());
                if (!seenRecords.Add(recordKey))
                {
                    errors.Add(new ImportValidationError(rowNumber, null, null,
                        "A duplicate record was detected in the file for the same county, occupation code, and insurance title."));
                }
            }
        }

        if (processedRows == 0)
        {
            errors.Add(new ImportValidationError(null, null, null, "The uploaded file does not contain any data rows to import."));
        }
    }
}
