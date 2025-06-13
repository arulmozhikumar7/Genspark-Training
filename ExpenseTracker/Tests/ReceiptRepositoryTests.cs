using ExpenseTrackerAPI.Data;
using ExpenseTrackerAPI.Models;
using ExpenseTrackerAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ExpenseTrackerAPI.Tests.Repositories
{
    public class ReceiptRepositoryTests
    {
        private readonly ExpenseTrackerDbContext _context;
        private readonly ReceiptRepository _repository;

        public ReceiptRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ExpenseTrackerDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ExpenseTrackerDbContext(options);
            _repository = new ReceiptRepository(_context);
        }


        [Fact]
        public async Task GetByExpenseIdAsync_ShouldReturnReceipts()
        {
            var expenseId = Guid.NewGuid();
            var receipts = new[]
            {
                new Receipt { ExpenseId = expenseId, FileName = "1.png" },
                new Receipt { ExpenseId = expenseId, FileName = "2.png" }
            };

            await _context.Receipts.AddRangeAsync(receipts);
            await _context.SaveChangesAsync();

            var result = await _repository.GetByExpenseIdAsync(expenseId);

            Assert.Equal(2, result.Count());
        }


        [Fact]
        public async Task DeleteAsync_ShouldSoftDeleteReceipt()
        {
            var receipt = new Receipt { ExpenseId = Guid.NewGuid(), FileName = "file.png" };
            _context.Receipts.Add(receipt);
            await _context.SaveChangesAsync();

            await _repository.DeleteAsync(receipt);
            await _repository.SaveChangesAsync();

            var updated = await _context.Receipts.FindAsync(receipt.Id);
            Assert.True(updated!.IsDeleted);
        }

        [Fact]
        public async Task SaveChangesAsync_ShouldReturnTrue_WhenChangesSaved()
        {
            var receipt = new Receipt { ExpenseId = Guid.NewGuid(), FileName = "file.png" };
            await _repository.AddAsync(receipt);
            var result = await _repository.SaveChangesAsync();

            Assert.True(result);
        }
    }
}
