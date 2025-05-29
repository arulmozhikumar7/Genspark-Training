using System.Threading.Tasks;
using BankingAPI.Models;

namespace BankingAPI.Interfaces
{
    public interface ITransaction
    {
        Task<Transaction> DepositAsync(int toAccountId, decimal amount, string? description = null);

        Task<Transaction> WithdrawAsync(int fromAccountId, decimal amount, string? description = null);

        Task<Transaction> TransferAsync(int fromAccountId, int toAccountId, decimal amount, string? description = null);
    }
}
