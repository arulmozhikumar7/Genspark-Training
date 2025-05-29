using System.ComponentModel.DataAnnotations;

namespace BankingAPI.DTOs
{
    public class TransactionRequestDto
    {
        public int? FromAccountId { get; set; }
        public int? ToAccountId { get; set; }

        [Range(1, double.MaxValue, ErrorMessage = "Amount must be at least 1.")]
        public decimal Amount { get; set; }

        public string? Description { get; set; }
    }
}
