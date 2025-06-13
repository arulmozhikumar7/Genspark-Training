using System.ComponentModel.DataAnnotations;

namespace ExpenseTrackerAPI.DTOs
{
   public class BudgetUpdateDto
   {
      public Guid Id { get; set; }

      public string? Name { get; set; }
      public Guid? CategoryId { get; set; }

      [Range(0.01, double.MaxValue, ErrorMessage = "Limit amount must be greater than 0.")]
      public decimal? LimitAmount { get; set; }
      public DateTime? StartDate { get; set; }
      public DateTime? EndDate { get; set; }
   }

}
