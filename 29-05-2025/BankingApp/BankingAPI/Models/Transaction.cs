using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankingAPI.Models
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public TransactionType Type { get; set; }

        [Required]
        public decimal Amount { get; set; }

        public DateTime Date { get; set; } = DateTime.UtcNow;

        public string? Description { get; set; }
        [Required]
        public TransactionStatus Status { get; set; } = TransactionStatus.Pending;
        public int? FromAccountId { get; set; }
        [ForeignKey("FromAccountId")]
        public BankAccount? FromAccount { get; set; }

        public int? ToAccountId { get; set; }
        [ForeignKey("ToAccountId")]
        public BankAccount? ToAccount { get; set; }
    }
}
