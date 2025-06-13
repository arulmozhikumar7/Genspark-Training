using System.ComponentModel.DataAnnotations;

namespace ExpenseTrackerAPI.DTOs
{
    public class BudgetDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;

        [Range(0.01, double.MaxValue, ErrorMessage = "Limit amount must be greater than 0.")]
        public decimal LimitAmount { get; set; }
        public decimal BalanceAmount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

}
