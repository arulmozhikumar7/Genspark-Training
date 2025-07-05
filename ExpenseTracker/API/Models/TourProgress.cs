using System;
using System.ComponentModel.DataAnnotations;

namespace ExpenseTrackerAPI.Models
{
    public class TourProgress
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid UserId { get; set; }

        public bool HomeTourCompleted { get; set; } = false;
        public bool BudgetTourCompleted { get; set; } = false;
        public bool ExpenseTourCompleted { get; set; } = false;
        public bool ReportTourCompleted { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
