using ClosedXML.Excel;
using LaborStats.Application.Abstractions;
using LaborStats.Application.Imports.Dtos;
using LaborStats.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Text;
using System.Globalization;

namespace LaborStats.Infrastructure.Services;

public class DataConversionService(LaborStatsDbContext dbContext) : IDataConversionService
{
    private const string PowiatColumn = "POWIAT";
    private const string OccupationCodeColumn = "KOD ZAWODU";
    private const string AllContractsColumn = "WSZYSTKICH UMÓW";
    private const string Over2YearsColumn = "POWYŻEJ 2 LAT";
    private const string NewlyRegisteredColumn = "NOWO ZAREJESTROWANY";

    private const string InsuredAllContracts = "INSURED_ALL_CONTRACTS";
    private const string InsuredOver2Years = "INSURED_OVER_2_YEARS";
    private const string InsuredNewlyRegistered = "INSURED_NEWLY_REGISTERED";

    public async Task<List<ConvertedImportRowDto>> ConvertAsync(Stream fileStream,
        string voivodeshipName,
        ImportDataType dataType,
        CancellationToken cancellationToken)
    {
        var resultList = new List<ConvertedImportRowDto>();

        using var workbook = new XLWorkbook(fileStream);
        var worksheet = workbook.Worksheet(1);

        string periodFromSheet = worksheet.Name.Trim();

        var headerMap = new Dictionary<string, int>();
        int headerRowIndex = 1;

        for (int r = 1; r <= 3; r++)
        {
            var testRow = worksheet.Row(r);
            bool hasPowiat = false;
            for (int col = 1; col <= testRow.LastCellUsed().Address.ColumnNumber; col++)
            {
                string text = GetCellStringValue(testRow.Cell(col)).ToUpper();
                if (text.Contains(PowiatColumn))
                {
                    hasPowiat = true;
                    break;
                }
            }
            if (hasPowiat)
            {
                headerRowIndex = r;
                break;
            }
        }

        var headerRow = worksheet.Row(headerRowIndex);
        for (int col = 1; col <= headerRow.LastCellUsed().Address.ColumnNumber; col++)
        {
            string headerText = GetCellStringValue(headerRow.Cell(col)).ToUpper();
            if (!string.IsNullOrEmpty(headerText))
            {
                headerMap[headerText] = col;
            }
        }

        int powiatCol = FindColumnIndex(headerMap, PowiatColumn);
        int professionCol = FindColumnIndex(headerMap, OccupationCodeColumn);

        int allContractsCol = 0;
        int over2YearsCol = 0;
        int newlyRegisteredCol = 0;

        if (dataType == ImportDataType.Employed)
        {
            allContractsCol = FindColumnIndex(headerMap, AllContractsColumn);
            over2YearsCol = FindColumnIndex(headerMap, Over2YearsColumn);
        }
        else if (dataType == ImportDataType.NewlyHired)
        {
            newlyRegisteredCol = FindColumnIndex(headerMap, NewlyRegisteredColumn);
        }

        var dbCounties = await dbContext.Counties
            .Where(c => c.Voivodeship.Name.ToLower() == voivodeshipName.ToLower())
            .ToListAsync(cancellationToken);

        var counties = dbCounties
            .GroupBy(c => c.Name.Trim())
            .ToDictionary(g => g.Key, g => g.First().Teryt, StringComparer.OrdinalIgnoreCase);

        var countiesNormalized = dbCounties
            .GroupBy(c => RemoveDiacritics(c.Name).ToLower().Trim())
            .ToDictionary(g => g.Key, g => g.First().Teryt, StringComparer.OrdinalIgnoreCase);

        var professionsByCode = await dbContext.Profession
            .ToDictionaryAsync(p => p.KzisCode, p => p.Id, cancellationToken);

        var professionsByName = await dbContext.Profession
            .GroupBy(p => p.KzisName.ToLower().Trim())
            .Select(g => new { Name = g.Key, Id = g.First().Id })
            .ToDictionaryAsync(x => x.Name, x => x.Id, cancellationToken);

        var rows = worksheet.RowsUsed().Skip(headerRowIndex);

        foreach (var row in rows)
        {
            string rawCounty = GetCellStringValue(row.Cell(powiatCol));
            string rawProfessionStr = GetCellProfessionCode(row.Cell(professionCol));

            if (string.IsNullOrWhiteSpace(rawCounty) && string.IsNullOrWhiteSpace(rawProfessionStr))
                continue;

            string countyId = string.Empty;
            if (!string.IsNullOrWhiteSpace(rawCounty))
            {
                if (counties.TryGetValue(rawCounty, out var foundTeryt))
                {
                    countyId = foundTeryt;
                }
                else
                {
                    string normCounty = RemoveDiacritics(rawCounty).ToLower().Trim();
                    if (countiesNormalized.TryGetValue(normCounty, out var foundNormTeryt))
                    {
                        countyId = foundNormTeryt;
                    }
                }
            }

            if (string.IsNullOrEmpty(countyId))
            {
                continue;
            }

            Guid professionId = Guid.Empty;

            if (!string.IsNullOrWhiteSpace(rawProfessionStr))
            {
                string digitsOnly = new string(rawProfessionStr.Where(char.IsDigit).ToArray());

                if (!string.IsNullOrEmpty(digitsOnly))
                {
                    if (int.TryParse(digitsOnly, out int parsedCode))
                    {
                        if (!professionsByCode.TryGetValue(parsedCode, out professionId))
                        {
                            if (digitsOnly.Length < 7)
                            {
                                string padded7 = digitsOnly.PadLeft(7, '0');
                                if (int.TryParse(padded7, out int parsed7Code))
                                {
                                    professionsByCode.TryGetValue(parsed7Code, out professionId);
                                }
                            }
                        }
                    }
                }

                if (professionId == Guid.Empty)
                {
                    professionsByName.TryGetValue(rawProfessionStr.ToLower().Trim(), out professionId);
                }
            }

            if (allContractsCol > 0)
            {
                int val = ParseFlexibleNumber(row.Cell(allContractsCol));
                resultList.Add(CreateRow(countyId, professionId, val, periodFromSheet, InsuredAllContracts));
            }

            if (over2YearsCol > 0)
            {
                int val = ParseFlexibleNumber(row.Cell(over2YearsCol));
                resultList.Add(CreateRow(countyId, professionId, val, periodFromSheet, InsuredOver2Years));
            }

            if (newlyRegisteredCol > 0)
            {
                int val = ParseFlexibleNumber(row.Cell(newlyRegisteredCol));
                resultList.Add(CreateRow(countyId, professionId, val, periodFromSheet, InsuredNewlyRegistered));
            }
        }

        return resultList;
    }

    private static ConvertedImportRowDto CreateRow(string countyId, Guid professionId, int value, string period, string dataType)
    {
        return new ConvertedImportRowDto
        {
            CountyId = countyId,
            ProfessionId = professionId,
            Value = value,
            Period = period,
            DataType = dataType
        };
    }

    private static string RemoveDiacritics(string text)
    {
        if (string.IsNullOrWhiteSpace(text)) return string.Empty;
        var normalizedString = text.Normalize(NormalizationForm.FormD);
        var stringBuilder = new StringBuilder();

        foreach (var c in normalizedString)
        {
            var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
            if (unicodeCategory != UnicodeCategory.NonSpacingMark)
            {
                stringBuilder.Append(c);
            }
        }

        return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
    }

    private static int FindColumnIndex(Dictionary<string, int> headerMap, string keyword)
    {
        var match = headerMap.FirstOrDefault(h => h.Key.Contains(keyword, StringComparison.OrdinalIgnoreCase));
        if (match.Key is null)
        {
            throw new InvalidOperationException($"Required column containing '{keyword}' was not found in the file.");
        }
        return match.Value;
    }

    private static string GetCellStringValue(IXLCell cell)
    {
        if (cell.IsEmpty()) return string.Empty;

        string text = cell.GetString()?.Trim() ?? string.Empty;
        if (cell.DataType == XLDataType.Number)
        {
            text = cell.GetText().Trim();
        }

        return text;
    }

    private static string GetCellProfessionCode(IXLCell cell)
    {
        if (cell.IsEmpty()) return string.Empty;

        try
        {
            if (cell.DataType == XLDataType.Number)
            {
                double num = cell.GetDouble();
                long longNum = (long)num;
                string strNum = longNum.ToString();
                
                if (strNum.Length < 7)
                {
                    return strNum.PadLeft(7, '0');
                }
                
                return strNum;
            }

            return cell.GetString()?.Trim() ?? string.Empty;
        }
        catch
        {
            return cell.GetString()?.Trim() ?? string.Empty;
        }
    }

    private static int ParseFlexibleNumber(IXLCell cell)
    {
        if (cell.IsEmpty()) return 0;

        if (cell.DataType == XLDataType.Number)
        {
            return (int)Math.Round(cell.GetValue<decimal>());
        }

        string input = cell.GetString()?.ToLower().Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(input)) return 0;

        decimal multiplier = 1m;
        if (input.Contains("tys")) multiplier = 1000m;
        else if (input.Contains("mln")) multiplier = 1_000_000m;
        else if (input.Contains("mld")) multiplier = 1_000_000_000m;

        var match = Regex.Match(input, @"[-+]?\d+([.,]\d+)?");
        if (match.Success)
        {
            string numberStr = match.Value.Replace(',', '.');
            if (decimal.TryParse(numberStr, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal parsedDecimal))
            {
                return (int)Math.Round(parsedDecimal * multiplier);
            }
        }

        return 0;
    }
}