namespace ExpenseTrackerAPI.DTOs
{
    public class BudgetQueryParameters
{
    public Guid? CategoryId { get; set; }
    public string? Search { get; set; }
    public int? Month { get; set; }
    public int? Year { get; set; }

    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

}