using BankingAPI.Models;
using BankingAPI.Interfaces;
using BankingAPI.Data;  
using BankingAPI.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BankingAPI.Repositories
{
    public class BankAccountRepository : IBankAccount
    {
        private readonly BankingDbContext _context;

        public BankAccountRepository(BankingDbContext context)
        {
            _context = context;
        }

   public async Task<BankAccount> AddAsync(int userId, AccountType accountType)
    {
        var sql = @"
            SELECT ba.""Id"", ba.""AccountNumber"", ba.""Balance"", ba.""UserId"", ba.""Status"", ba.""AccountType""
            FROM create_bank_account({0}, {1}) AS ba(""Id"", ""AccountNumber"", ""Balance"", ""UserId"", ""Status"", ""AccountType"")
            LIMIT 1";

        var bankAccount = await _context.Set<BankAccount>()
            .FromSqlRaw(sql, userId, (int)accountType)
            .AsNoTracking()
            .FirstOrDefaultAsync();

        return bankAccount!;
    }

    public async Task<decimal?> GetAvailableBalanceAsync(string accountNumber)
        {
            var account = await _context.BankAccounts
                .AsNoTracking()
                .FirstOrDefaultAsync(acc => acc.AccountNumber == accountNumber);

            return account?.Balance;
        }
    }
}
