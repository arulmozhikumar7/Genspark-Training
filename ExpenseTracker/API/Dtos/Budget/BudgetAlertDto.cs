namespace ExpenseTrackerAPI.Dtos
{
    public class BudgetAlertDto
    {
        public Guid BudgetId { get; set; }
        public string Name { get; set; } = null!;
        public string CategoryName { get; set; } = null!;
        public decimal LimitAmount { get; set; }
        public decimal UsedAmount { get; set; }
        public int UsedPercentage { get; set; }
        public string Message { get; set; } = null!;
    }
}