using BankingAPI.DTOs;
using BankingAPI.Models;

namespace BankingAPI.Mappers
{
    public static class TransactionMapper
    {
        public static TransactionResponseDto ToDto(Transaction txn)
        {
            return new TransactionResponseDto
            {
                Id = txn.Id,
                Type = txn.Type.ToString(),
                Amount = txn.Amount,
                Status = txn.Status.ToString(),
                Date = txn.Date
            };
        }
    }
}
