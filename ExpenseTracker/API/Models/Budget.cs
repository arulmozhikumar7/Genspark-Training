using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTrackerAPI.Models
{
    public class Budget
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public Guid? UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User? User { get; set; } = null!;

        [Required]
        public Guid CategoryId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; } = null!;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal LimitAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal BalanceAmount { get; set; }  

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public bool IsDeleted { get; set; } = false;

       public bool Notified50Percent { get; set; } = false;
      public bool Notified90Percent { get; set; } = false;
      public bool NotifiedLimitExceeded { get; set; } = false;
    }
}
