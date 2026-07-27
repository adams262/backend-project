using System.Data;
using System.Text.RegularExpressions;
using ExcelDataReader;
using LaborStats.Application.Abstractions;
using LaborStats.Application.Imports;
using LaborStats.Application.Imports.Dtos;
using LaborStats.Domain.Entities;
using LaborStats.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LaborStats.Infrastructure.Services;

public sealed partial class ImportValidator : IImportValidator
{
    private readonly LaborStatsDbContext _context;

    private const string Employed = "pracujący";
    private const string NewlyHired = "nowozatrudnieni";

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

        if (request.File == null || request.File.Length == 0)
        {
            errors.Add(new ImportValidationError(null, "File", null, "The import file is required and cannot be empty."));
            return new ImportValidationResult(false, errors);
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

        bool isEmployed = string.Equals(request.DataType, Employed, StringComparison.OrdinalIgnoreCase);
        bool isNewlyHired = string.Equals(request.DataType, NewlyHired, StringComparison.OrdinalIgnoreCase);

        if (!isEmployed && !isNewlyHired)
        {
            errors.Add(new ImportValidationError(null, "DataType", request.DataType,
                $"Data type '{request.DataType}' is not recognized. Expected 'pracujący' or 'nowozatrudnieni'."));
        }

        if (errors.Count > 0)
        {
            return new ImportValidationResult(false, errors);
        }

        string[] requiredColumns = isEmployed
            ? ["WOJEWODZTWO", "POWIAT", "KOD ZAWODU UBEZPIECZONEGO", "KOD TYTULU UBEZPIECZENIA",
               "LICZBA UBEZPIECZONYCH WSZYSTKICH UMOW", "LICZBA UBEZPIECZONYCH UMOW POWYZEJ 2 LAT"]
            : ["WOJEWODZTWO", "POWIAT", "KOD ZAWODU UBEZPIECZONEGO", "KOD TYTULU UBEZPIECZENIA",
               "LICZBA UBEZPIECZONYCH (UMOW) - NOWO ZAREJESTROWANYCH"];

        using var stream = request.File.OpenReadStream();
        using var reader = ExcelReaderFactory.CreateReader(stream);

        var columnIndex = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        bool headerFound = false;
        int rowNumber = 0;

        while (rowNumber < 5 && reader.Read())
        {
            rowNumber++;

            for (int i = 0; i < reader.FieldCount; i++)
            {
                string val = NormalizeHeader(reader.GetValue(i)?.ToString() ?? string.Empty);
                if (val.Contains("WOJEW", StringComparison.OrdinalIgnoreCase))
                {
                    for (int j = 0; j < reader.FieldCount; j++)
                    {
                        string header = NormalizeHeader(reader.GetValue(j)?.ToString() ?? string.Empty);
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
            errors.Add(new ImportValidationError(null, null, null, "The file does not contain a valid header row with the 'WOJEWÓDZTWO' column."));
            return new ImportValidationResult(false, errors);
        }

        foreach (var required in requiredColumns)
        {
            bool found = columnIndex.Keys.Any(k => k.StartsWith(required, StringComparison.OrdinalIgnoreCase));
            if (!found)
            {
                errors.Add(new ImportValidationError(null, required, null, $"Required column '{required}' was not found in the file."));
            }
        }

        if (errors.Count > 0)
        {
            return new ImportValidationResult(false, errors);
        }

        var knownCounties = new HashSet<string>(
            await _context.Counties.AsNoTracking()
                .Where(c => c.Voivodeship.Name.ToLower() == request.Voivodeship.ToLower())
                .Select(c => c.Name)
                .ToListAsync(cancellationToken),
            StringComparer.OrdinalIgnoreCase);

        var knownProfessions = await _context.Profession.AsNoTracking()
            .Select(p => new { p.KzisCode })
            .ToListAsync(cancellationToken);

        var knownOccupationCodes = new HashSet<int>(knownProfessions.Select(p => p.KzisCode));

        var seenRecords = new HashSet<(string County, int Code, string Title)>();

        string numericColumn = isEmployed
            ? "LICZBA UBEZPIECZONYCH WSZYSTKICH UMOW"
            : "LICZBA UBEZPIECZONYCH (UMOW) - NOWO ZAREJESTROWANYCH";

        int voivodeshipIdx = FindColumn(columnIndex, "WOJEWODZTWO");
        int countyIdx = FindColumn(columnIndex, "POWIAT");
        int occupationIdx = FindColumn(columnIndex, "KOD ZAWODU UBEZPIECZONEGO");
        int insuranceTitleIdx = FindColumn(columnIndex, "KOD TYTULU UBEZPIECZENIA");
        int numericIdx = FindColumn(columnIndex, numericColumn);

        int processedRows = 0;

        while (reader.Read())
        {
            rowNumber++;

            string voivodeshipCell = GetString(reader, voivodeshipIdx);
            string cleanedVoivodeship = voivodeshipCell.Replace(",", "").Trim();

            if (string.IsNullOrWhiteSpace(cleanedVoivodeship) || cleanedVoivodeship == "-")
            {
                continue;
            }

            processedRows++;

            string county = GetString(reader, countyIdx);
            string occupationStr = GetString(reader, occupationIdx);
            string insuranceTitle = GetString(reader, insuranceTitleIdx);
            string numericValue = GetString(reader, numericIdx);

            if (string.IsNullOrWhiteSpace(county))
            {
                errors.Add(new ImportValidationError(rowNumber, "POWIAT", null, "The county field is required."));
            }
            else if (!knownCounties.Contains(county))
            {
                errors.Add(new ImportValidationError(rowNumber, "POWIAT", county,
                    $"County '{county}' does not belong to the specified voivodeship '{request.Voivodeship}' or does not exist."));
            }

            int occupationCode = 0;
            if (string.IsNullOrWhiteSpace(occupationStr))
            {
                errors.Add(new ImportValidationError(rowNumber, "KOD ZAWODU UBEZPIECZONEGO", null, "Occupation code is required."));
            }
            else if (!int.TryParse(occupationStr, out occupationCode))
            {
                errors.Add(new ImportValidationError(rowNumber, "KOD ZAWODU UBEZPIECZONEGO", occupationStr,
                    "Occupation code is not in a valid numeric format."));
            }
            else if (!knownOccupationCodes.Contains(occupationCode))
            {
                errors.Add(new ImportValidationError(rowNumber, "KOD ZAWODU UBEZPIECZONEGO", occupationStr,
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

        return new ImportValidationResult(errors.Count == 0, errors);
    }

    private static string NormalizeHeader(string rawHeader)
    {
        string header = rawHeader.Replace("\n", " ").Replace("\r", " ").Trim();
        header = Regex.Replace(header, @"\s+", " ");
        return RemoveDiacritics(header);
    }

    private static string RemoveDiacritics(string text)
    {
        if (string.IsNullOrEmpty(text)) return text;
        var src = "ąęćłńóśźżĄĘĆŁŃÓŚŹŻ";
        var dst = "aeclnoszzAECLNOSZZ";
        for (int i = 0; i < src.Length; i++)
        {
            text = text.Replace(src[i], dst[i]);
        }
        return text;
    }

    private static int FindColumn(Dictionary<string, int> columnIndex, string prefix)
    {
        string normalizedPrefix = RemoveDiacritics(prefix);
        var match = columnIndex.FirstOrDefault(kv =>
            kv.Key.StartsWith(normalizedPrefix, StringComparison.OrdinalIgnoreCase));
        return match.Key is not null ? match.Value : -1;
    }

    private static string GetString(IExcelDataReader reader, int index)
    {
        if (index < 0 || reader.IsDBNull(index))
            return string.Empty;

        return reader.GetValue(index)?.ToString()?.Trim() ?? string.Empty;
    }
}
