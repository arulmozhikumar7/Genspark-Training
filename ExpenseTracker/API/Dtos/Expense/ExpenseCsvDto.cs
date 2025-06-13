using System.ComponentModel.DataAnnotations;

namespace ExpenseTrackerAPI.DTOs
{
    public class ExpenseCsvDto
    {
        public DateTime ExpenseDate { get; set; }
        public string CategoryName { get; set; } = "";

        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0.")]
        public decimal Amount { get; set; }
        public string? Description { get; set; }
    }

}