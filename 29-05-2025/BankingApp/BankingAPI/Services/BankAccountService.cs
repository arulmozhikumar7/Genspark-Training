using BankingAPI.DTOs;
using BankingAPI.Mappers;
using BankingAPI.Models;
using BankingAPI.Repositories;
using BankingAPI.Interfaces;

namespace BankingAPI.Services
{
    public class BankAccountService
    {
        private readonly IBankAccount _repository;

        public BankAccountService(IBankAccount repository)
        {
            _repository = repository;
        }

        public async Task<BankAccount> CreateBankAccountAsync(int userId, AccountType accountType)
        {
            var bankAccountDto = await _repository.AddAsync(userId, accountType);

            if (bankAccountDto == null)
                throw new Exception("Failed to create bank account or user not found.");

            return bankAccountDto;
        }

        
         public async Task<decimal?> GetAvailableBalanceAsync(string accountNumber)
        {
            var balance = await _repository.GetAvailableBalanceAsync(accountNumber);

            if (balance == null)
                throw new Exception($"Bank account with account number {accountNumber} not found.");

            return balance;
        }
    }
}
