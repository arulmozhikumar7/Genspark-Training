using System.Globalization;
using System.Text;
using ExpenseTrackerAPI.DTOs;

namespace ExpenseTrackerAPI.Utils{
    public class CsvGenerator
{
    public string GenerateExpensesCsv(IEnumerable<ExpenseCsvDto> expenses)
    {
        var sb = new StringBuilder();
        sb.AppendLine("Date,Category,Amount,Description");

        foreach (var e in expenses)
        {
            var date = e.ExpenseDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            var category = EscapeCsv(e.CategoryName);
            var amount = e.Amount.ToString("F2", CultureInfo.InvariantCulture);
            var description = EscapeCsv(e.Description ?? "");

            sb.AppendLine($"{date},{category},{amount},{description}");
        }

        return sb.ToString();
    }

    private string EscapeCsv(string value)
    {
        if (value.Contains(",") || value.Contains("\n") || value.Contains("\""))
        {
            value = value.Replace("\"", "\"\"");
            return $"\"{value}\"";
        }
        return value;
    }
}
}