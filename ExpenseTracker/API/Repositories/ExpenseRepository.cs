using ExpenseTrackerAPI.Data;
using ExpenseTrackerAPI.Interfaces;
using ExpenseTrackerAPI.Models;
using ExpenseTrackerAPI.DTOs;
using Microsoft.EntityFrameworkCore;


namespace ExpenseTrackerAPI.Repositories
{
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly ExpenseTrackerDbContext _context;

        public ExpenseRepository(ExpenseTrackerDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Expense>> GetAllByUserIdAsync(Guid userId)
        {
            return await _context.Expenses
                .Include(e => e.Category)
                .Where(e => e.UserId == userId && !e.IsDeleted)
                .ToListAsync();
        }

        public IQueryable<Expense> GetAllByUserIdQueryable(Guid userId)
        {
            return _context.Expenses
                .Where(e => e.UserId == userId && !e.IsDeleted)
                .OrderByDescending(e => e.ExpenseDate);
        }
        public async Task<Expense?> GetByIdAsync(Guid id, Guid userId)
        {
            return await _context.Expenses
                .Include(e => e.Category)
                .FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId && !e.IsDeleted);
        }

        public async Task AddAsync(Expense expense)
        {
            await _context.Expenses.AddAsync(expense);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Expense expense)
        {
            _context.Expenses.Update(expense);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id, Guid userId)
        {
            var expense = await GetByIdAsync(id, userId);
            if (expense is null) throw new KeyNotFoundException("Expense not found");
            expense.IsDeleted = true;
            await _context.SaveChangesAsync();
        }

        public async Task<(IEnumerable<Expense> Expenses, int TotalCount)> GetFilteredAsync(Guid userId, ExpenseQueryParameters parameters)
        {
            var query = _context.Expenses
                .Include(e => e.Category)
                .Where(e => e.UserId == userId && !e.IsDeleted);

            if (parameters.CategoryId.HasValue)
                query = query.Where(e => e.CategoryId == parameters.CategoryId);

            if (parameters.MinAmount.HasValue)
                query = query.Where(e => e.Amount >= parameters.MinAmount);

            if (parameters.MaxAmount.HasValue)
                query = query.Where(e => e.Amount <= parameters.MaxAmount);

            if (parameters.FromDate.HasValue)
                query = query.Where(e => e.ExpenseDate >= parameters.FromDate);

            if (parameters.ToDate.HasValue)
                query = query.Where(e => e.ExpenseDate <= parameters.ToDate);

           if (!string.IsNullOrWhiteSpace(parameters.Search))
            {
                var search = $"%{parameters.Search}%";
                query = query.Where(e => EF.Functions.ILike(e.Description!, search));
            }

            query = parameters.SortBy?.ToLower() switch
            {
                "amount" => parameters.SortDirection == "asc" ? query.OrderBy(e => e.Amount) : query.OrderByDescending(e => e.Amount),
                "category" => parameters.SortDirection == "asc" ? query.OrderBy(e => e.Category.Name) : query.OrderByDescending(e => e.Category.Name),
                _ => parameters.SortDirection == "asc" ? query.OrderBy(e => e.ExpenseDate) : query.OrderByDescending(e => e.ExpenseDate)
            };

            var total = await query.CountAsync();
            var expenses = await query
                .Skip((parameters.Page - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();

            return (expenses, total);
        }

    }
}