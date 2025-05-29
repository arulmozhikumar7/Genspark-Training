using BankingAPI.Models;

namespace BankingAPI.DTOs
{
    public class BankAccountCreateDto
    {
        public int UserId { get; set; }
        public AccountType AccountType { get; set; }
    }
}
