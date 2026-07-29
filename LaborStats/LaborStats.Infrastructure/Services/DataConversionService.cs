using ClosedXML.Excel;
using LaborStats.Application.Abstractions;
using LaborStats.Application.Imports.Dtos;
using LaborStats.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text.RegularExpressions;

namespace LaborStats.Infrastructure.Services;

public class DataConversionService(LaborStatsDbContext dbContext) : IDataConversionService
{
    public async Task<List<ConvertedImportRowDto>> ConvertAsync(Stream fileStream, CancellationToken cancellationToken)
    {
        var resultList = new List<ConvertedImportRowDto>();

        using var workbook = new XLWorkbook(fileStream);
        var worksheet = workbook.Worksheet(1);
        
        string periodFromSheet = worksheet.Name.Trim();

        var headerRow = worksheet.Row(1);
        var headerMap = new Dictionary<string, int>();

        for (int col = 1; col <= headerRow.LastCellUsed().Address.ColumnNumber; col++)
        {
            string headerText = headerRow.Cell(col).GetFormattedString().Trim().ToUpper();
            headerMap[headerText] = col;
        }

        int powiatCol = FindColumnIndex(headerMap, "POWIAT");
        int professionCol = FindColumnIndex(headerMap, "KOD ZAWODU");
        
        int allContractsCol = FindColumnIndex(headerMap, "WSZYSTKICH UMÓW");
        int over2YearsCol = FindColumnIndex(headerMap, "POWYŻEJ 2 LAT");
        int newlyRegisteredCol = FindColumnIndex(headerMap, "NOWO ZAREJESTROWANY");

        var counties = await dbContext.Counties
            .ToDictionaryAsync(c => c.Name.ToLower().Trim(), c => c.Teryt, cancellationToken);
            
        var professionsByCode = await dbContext.Profession
            .ToDictionaryAsync(p => p.KzisCode, p => p.Id, cancellationToken);

        var professionsByName = await dbContext.Profession
            .ToDictionaryAsync(p => p.KzisName.ToLower().Trim(), p => p.Id, cancellationToken);

        var rows = worksheet.RowsUsed().Skip(1);

        foreach (var row in rows)
        {
            string rawCounty = GetCellStringValue(row.Cell(powiatCol));
            string rawProfessionStr = GetCellStringValue(row.Cell(professionCol));

            if (string.IsNullOrWhiteSpace(rawCounty) && string.IsNullOrWhiteSpace(rawProfessionStr))
                continue;

            string countyId = counties.TryGetValue(rawCounty.ToLower(), out var foundTeryt) ? foundTeryt : string.Empty;

            Guid professionId = Guid.Empty;
            if (int.TryParse(rawProfessionStr, out int kzisCode))
            {
                professionsByCode.TryGetValue(kzisCode, out professionId);
            }
            else
            {
                professionsByName.TryGetValue(rawProfessionStr.ToLower(), out professionId);
            }

            if (allContractsCol > 0)
            {
                int val = ParseFlexibleNumber(row.Cell(allContractsCol));
                resultList.Add(CreateRow(countyId, professionId, val, periodFromSheet, "INSURED_ALL_CONTRACTS"));
            }

            if (over2YearsCol > 0)
            {
                int val = ParseFlexibleNumber(row.Cell(over2YearsCol));
                resultList.Add(CreateRow(countyId, professionId, val, periodFromSheet, "INSURED_OVER_2_YEARS"));
            }

            if (newlyRegisteredCol > 0)
            {
                int val = ParseFlexibleNumber(row.Cell(newlyRegisteredCol));
                resultList.Add(CreateRow(countyId, professionId, val, periodFromSheet, "INSURED_NEWLY_REGISTERED"));
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

    private static int FindColumnIndex(Dictionary<string, int> headerMap, string keyword)
    {
        var match = headerMap.FirstOrDefault(h => h.Key.Contains(keyword, StringComparison.OrdinalIgnoreCase));
        return match.Value;
    }

    private static string GetCellStringValue(IXLCell cell)
    {
        if (cell.IsEmpty()) return string.Empty;
        return cell.GetValue<string>()?.Trim() ?? string.Empty;
    }

    private static int ParseFlexibleNumber(IXLCell cell)
    {
        if (cell.IsEmpty()) return 0;

        if (cell.DataType == XLDataType.Number)
        {
            return (int)Math.Round(cell.GetValue<decimal>());
        }

        string input = cell.GetValue<string>()?.ToLower().Trim() ?? string.Empty;
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