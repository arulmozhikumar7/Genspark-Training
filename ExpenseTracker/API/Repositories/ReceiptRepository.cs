using ExpenseTrackerAPI.Data;
using ExpenseTrackerAPI.Interfaces;
using ExpenseTrackerAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTrackerAPI.Repositories
{
    public class ReceiptRepository : IReceiptRepository
    {
        private readonly ExpenseTrackerDbContext _context;

        public ReceiptRepository(ExpenseTrackerDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Receipt>> GetByExpenseIdAsync(Guid expenseId)
        {
            return await _context.Receipts
                .Where(r => r.ExpenseId == expenseId && !r.IsDeleted)
                .ToListAsync();
        }

        public async Task<Receipt?> GetByIdAsync(Guid id)
        {
             return await _context.Receipts
        .Include(r => r.Expense)
        .FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted);
        }

        public async Task AddAsync(Receipt receipt)
        {
            await _context.Receipts.AddAsync(receipt);
        }
        public Task DeleteAsync(Receipt receipt)
        {
            receipt.IsDeleted = true;
            _context.Receipts.Update(receipt);
            return Task.CompletedTask;
        }



        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}