using BankingAPI.DTOs;
using BankingAPI.Interfaces;
using BankingAPI.Mappers;
using BankingAPI.Models;

namespace BankingAPI.Services
{
    public class TransactionService
    {
        private readonly ITransaction _transactionRepo;

        public TransactionService(ITransaction transactionRepo)
        {
            _transactionRepo = transactionRepo;
        }

        public async Task<TransactionResponseDto> DepositAsync(TransactionRequestDto dto)
        {
            var txn = await _transactionRepo.DepositAsync(dto.ToAccountId!.Value, dto.Amount, dto.Description);
            return TransactionMapper.ToDto(txn);
        }

        public async Task<TransactionResponseDto> WithdrawAsync(TransactionRequestDto dto)
        {
            var txn = await _transactionRepo.WithdrawAsync(dto.FromAccountId!.Value, dto.Amount, dto.Description);
            return TransactionMapper.ToDto(txn);
        }

        public async Task<TransactionResponseDto> TransferAsync(TransactionRequestDto dto)
        {
            var txn = await _transactionRepo.TransferAsync(dto.FromAccountId!.Value, dto.ToAccountId!.Value, dto.Amount, dto.Description);
            return TransactionMapper.ToDto(txn);
        }
    }
}