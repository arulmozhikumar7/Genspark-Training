using System;
using System.Threading.Tasks;
using BankingAPI.Data;
using BankingAPI.Interfaces;
using BankingAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BankingAPI.Repositories
{
    public class TransactionRepository : ITransaction
    {
        private readonly BankingDbContext _context;

        public TransactionRepository(BankingDbContext context)
        {
            _context = context;
        }

    public async Task<Transaction> DepositAsync(int toAccountId, decimal amount, string? description = null)
{
    using var dbTransaction = await _context.Database.BeginTransactionAsync();
    Transaction? failedTxn = null;

    try
    {
        var toAccount = await _context.BankAccounts.FindAsync(toAccountId);
        if (toAccount == null)
            throw new ArgumentException("Destination account not found.");

        toAccount.Balance += amount;

        var txn = new Transaction
        {
            Type = TransactionType.Deposit,
            Amount = amount,
            ToAccountId = toAccountId,
            Description = description,
            Status = TransactionStatus.Pending
        };

        _context.Transactions.Add(txn);
        await _context.SaveChangesAsync();

        txn.Status = TransactionStatus.Completed;
        await _context.SaveChangesAsync();
        await dbTransaction.CommitAsync();

        return txn;
    }
    catch (ArgumentException) 
    {
        await dbTransaction.RollbackAsync();
        throw;
    }
    catch
    {
        await dbTransaction.RollbackAsync();

        
        var toAccountExists = await _context.BankAccounts.AnyAsync(a => a.Id == toAccountId);
        if (toAccountExists)
        {
            failedTxn = new Transaction
            {
                Type = TransactionType.Deposit,
                Amount = amount,
                ToAccountId = toAccountId,
                Description = description,
                Status = TransactionStatus.Failed
            };

            _context.Transactions.Add(failedTxn);
            await _context.SaveChangesAsync();
        }

        return failedTxn!;
    }
}


    public async Task<Transaction> WithdrawAsync(int fromAccountId, decimal amount, string? description = null)
{
    using var transaction = await _context.Database.BeginTransactionAsync();
    Transaction? failedTxn = null;

    try
    {
        var fromAccount = await _context.BankAccounts.FindAsync(fromAccountId);
        if (fromAccount == null)
            throw new ArgumentException("Source account not found.");
        if (fromAccount.Balance < amount)
            throw new InvalidOperationException("Insufficient balance.");

        fromAccount.Balance -= amount;

        var txn = new Transaction
        {
            Type = TransactionType.Withdrawal,
            Amount = amount,
            FromAccountId = fromAccountId,
            Description = description,
            Status = TransactionStatus.Pending
        };

        _context.Transactions.Add(txn);
        await _context.SaveChangesAsync();

        txn.Status = TransactionStatus.Completed;
        await _context.SaveChangesAsync();
        await transaction.CommitAsync();

        return txn;
    }
    catch
    {
        await transaction.RollbackAsync();

        failedTxn = new Transaction
        {
            Type = TransactionType.Withdrawal,
            Amount = amount,
            FromAccountId = fromAccountId,
            Description = description,
            Status = TransactionStatus.Failed
        };
    }

    _context.Transactions.Add(failedTxn!);
    await _context.SaveChangesAsync();

    return failedTxn!;
}

       public async Task<Transaction> TransferAsync(int fromAccountId, int toAccountId, decimal amount, string? description = null)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        Transaction? failedTxn = null;

        try
        {
            if (fromAccountId == toAccountId)
                throw new ArgumentException("Cannot transfer to the same account.");

            var fromAccount = await _context.BankAccounts.FindAsync(fromAccountId);
            var toAccount = await _context.BankAccounts.FindAsync(toAccountId);

            if (fromAccount == null || toAccount == null)
                throw new ArgumentException("One or both accounts not found.");

            if (fromAccount.Balance < amount)
                throw new InvalidOperationException("Insufficient balance.");

            fromAccount.Balance -= amount;
            toAccount.Balance += amount;

            var txn = new Transaction
            {
                Type = TransactionType.Transfer,
                Amount = amount,
                FromAccountId = fromAccountId,
                ToAccountId = toAccountId,
                Description = description,
                Status = TransactionStatus.Pending
            };

            _context.Transactions.Add(txn);
            await _context.SaveChangesAsync();

            txn.Status = TransactionStatus.Completed;
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return txn;
        }
        catch
        {
            await transaction.RollbackAsync();

           
            failedTxn = new Transaction
            {
                Type = TransactionType.Transfer,
                Amount = amount,
                FromAccountId = fromAccountId,
                ToAccountId = toAccountId,
                Description = description,
                Status = TransactionStatus.Failed
            };
        }

       
        _context.Transactions.Add(failedTxn!);
        await _context.SaveChangesAsync();

        return failedTxn!;
    }

        }
}
