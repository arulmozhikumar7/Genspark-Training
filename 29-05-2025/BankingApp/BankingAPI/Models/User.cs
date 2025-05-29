using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BankingAPI.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!; 

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;  

        public string PhoneNumber { get; set; } = null!; 

        public AccountStatus Status { get; set; } = AccountStatus.Active;

        public ICollection<BankAccount> BankAccounts { get; set; } = new List<BankAccount>();  
    }
}
