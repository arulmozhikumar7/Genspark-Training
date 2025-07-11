namespace ExpenseTrackerAPI.DTOs{
    public class MonthlyCategoryComparisonDto
{
    public string CategoryName { get; set; } = string.Empty;
    public decimal Month1Amount { get; set; }
    public decimal Month2Amount { get; set; }
    public decimal Difference => Month2Amount - Month1Amount;
    public double PercentageChange =>
        Month1Amount == 0
            ? Month2Amount == 0 ? 0 : 100
            : Math.Round((double)(Difference / Month1Amount) * 100, 2);
}

public class MonthlyExpenseComparisonSummaryDto
{
    public int Year1 { get; set; }
    public int Month1 { get; set; }
    public int Year2 { get; set; }
    public int Month2 { get; set; }
    public decimal TotalMonth1 { get; set; }
    public decimal TotalMonth2 { get; set; }
    public decimal TotalDifference => TotalMonth2 - TotalMonth1;
    public double TotalPercentageChange =>
        TotalMonth1 == 0
            ? TotalMonth2 == 0 ? 0 : 100
            : Math.Round((double)(TotalDifference / TotalMonth1) * 100, 2);
    public List<MonthlyCategoryComparisonDto> CategoryComparisons { get; set; } = new();
}

}