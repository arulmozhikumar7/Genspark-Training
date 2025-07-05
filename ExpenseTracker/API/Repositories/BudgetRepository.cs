using ExpenseTrackerAPI.Data;
using ExpenseTrackerAPI.Interfaces;
using ExpenseTrackerAPI.Models;
using Microsoft.EntityFrameworkCore;
using ExpenseTrackerAPI.DTOs;
namespace ExpenseTrackerAPI.Repositories
{
    public class BudgetRepository : IBudgetRepository
    {
        private readonly ExpenseTrackerDbContext _context;

        public BudgetRepository(ExpenseTrackerDbContext context)
        {
            _context = context;
        }

        public async Task<(IEnumerable<Budget> Budgets, int TotalCount)> GetFilteredAsync(Guid userId, BudgetQueryParameters filters)
{
    var query = _context.Budgets
        .Where(b => b.UserId == userId && !b.IsDeleted)
        .Include(b => b.Category)
        .AsQueryable();

    if (filters.CategoryId.HasValue)
    {
        query = query.Where(b => b.CategoryId == filters.CategoryId);
    }

    if (!string.IsNullOrWhiteSpace(filters.Search))
    {
        var search = filters.Search.ToLower();
        query = query.Where(b =>
            b.Name.ToLower().Contains(search) ||
            b.Category.Name.ToLower().Contains(search));
    }

    if (filters.Month.HasValue)
    {
        query = query.Where(b => b.StartDate.Month == filters.Month || b.EndDate.Month == filters.Month);
    }

    if (filters.Year.HasValue)
    {
        query = query.Where(b => b.StartDate.Year == filters.Year || b.EndDate.Year == filters.Year);
    }

    var total = await query.CountAsync();

    var paged = await query
        .Skip((filters.Page - 1) * filters.PageSize)
        .Take(filters.PageSize)
        .ToListAsync();

    return (paged, total);
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