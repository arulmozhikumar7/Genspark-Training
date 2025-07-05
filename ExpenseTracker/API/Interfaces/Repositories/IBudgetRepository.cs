using ExpenseTrackerAPI.Models;
using ExpenseTrackerAPI.DTOs;

namespace ExpenseTrackerAPI.Interfaces
{
    public interface IBudgetRepository
    {
        Task<(IEnumerable<Budget> Budgets, int TotalCount)> GetFilteredAsync(Guid userId, BudgetQueryParameters filters);
        Task<Budget?> GetByIdAsync(Guid id, Guid userId);
        Task AddAsync(Budget budget);
        Task UpdateAsync(Budget budget);
        Task DeleteAsync(Budget budget);
        Task<bool> SaveChangesAsync();
    }

}
