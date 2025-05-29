using System.Threading.Tasks;
using BankingAPI.Models;

namespace BankingAPI.Interfaces
{
    public interface IBankAccount
    {
       Task<BankAccount> AddAsync(int userId, AccountType accountType);
        Task<decimal?> GetAvailableBalanceAsync(string accountNumber);

    }
}
