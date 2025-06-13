using ExpenseTrackerAPI.Models;
using ExpenseTrackerAPI.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTrackerAPI.Interfaces
{
    public interface IExpenseRepository
    {
        Task<IEnumerable<Expense>> GetAllByUserIdAsync(Guid userId);
        IQueryable<Expense> GetAllByUserIdQueryable(Guid userId);
        Task<Expense?> GetByIdAsync(Guid id, Guid userId);
        Task AddAsync(Expense expense);
        Task UpdateAsync(Expense expense);
        Task DeleteAsync(Guid id, Guid userId);
        Task<(IEnumerable<Expense> Expenses, int TotalCount)> GetFilteredAsync(Guid userId, ExpenseQueryParameters parameters);

    }

}