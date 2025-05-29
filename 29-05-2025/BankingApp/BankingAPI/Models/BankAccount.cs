using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankingAPI.Models
{
    public class BankAccount
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string AccountNumber { get; set; } = string.Empty;

        [Required]
        public decimal Balance { get; set; } = 0;

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; } = null!;

        public AccountStatus Status { get; set; } = AccountStatus.Active;
         [Required]
        public AccountType AccountType { get; set; } = AccountType.Savings;

        public ICollection<Transaction> FromTransactions { get; set; } = new List<Transaction>();
        public ICollection<Transaction> ToTransactions { get; set; } = new List<Transaction>();
    }
}
