using ExpenseTrackerAPI.Data;
using ExpenseTrackerAPI.Interfaces;
using ExpenseTrackerAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTrackerAPI.Repositories
{
    public class BudgetRepository : IBudgetRepository
    {
        private readonly ExpenseTrackerDbContext _context;

        public BudgetRepository(ExpenseTrackerDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Budget>> GetAllAsync(Guid userId)
        {
            return await _context.Budgets
                .Where(b => b.UserId == userId && !b.IsDeleted)
                .Include(b => b.Category)
                .ToListAsync();
        }

        public async Task<Budget?> GetByIdAsync(Guid id, Guid userId)
        {
            return await _context.Budgets
                .Include(b => b.Category)
                .FirstOrDefaultAsync(b => b.Id == id && b.UserId == userId && !b.IsDeleted);
        }

        public async Task AddAsync(Budget budget) => await _context.Budgets.AddAsync(budget);

        public Task UpdateAsync(Budget budget)
        {
            _context.Budgets.Update(budget);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Budget budget)
        {
            budget.IsDeleted = true;
            _context.Budgets.Update(budget);
            return Task.CompletedTask;
        }

        public async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() > 0;
    }
}