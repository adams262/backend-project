
using System;
using System.Collections.Generic;
using System.Text;
using ExcelDataReader;

namespace LaborStats.Infrastructure.Helpers;

public static class ExcelHelper
{
    public static string NormalizeHeader(string rawHeader)
    {
        string header = rawHeader.Replace("\n", " ").Replace("\r", " ").Trim();
        header = System.Text.RegularExpressions.Regex.Replace(header, @"\s+", " ");
        return RemoveDiacritics(header);
    }

    public static string RemoveDiacritics(string text)
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

    public static int FindColumn(Dictionary<string, int> columnIndex, string prefix)
    {
        string normalizedPrefix = RemoveDiacritics(prefix);
        var match = columnIndex.FirstOrDefault(kv =>
            kv.Key.StartsWith(normalizedPrefix, StringComparison.OrdinalIgnoreCase));
        return match.Key is not null ? match.Value : -1;
    }

    public static string GetString(IExcelDataReader reader, int index)
    {
        if (index < 0 || reader.IsDBNull(index))
            return string.Empty;

        return reader.GetValue(index)?.ToString()?.Trim() ?? string.Empty;
    }
}
