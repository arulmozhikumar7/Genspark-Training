using ExpenseTrackerAPI.DTOs;
using ExpenseTrackerAPI.Models;

namespace ExpenseTrackerAPI.Interfaces
{
    public interface IBudgetService
    {
        Task<IEnumerable<BudgetDto>> GetAllAsync(Guid userId);
        Task<BudgetDto?> GetByIdAsync(Guid id, Guid userId);
        Task CreateAsync(Guid userId, BudgetCreateDto dto);
        Task UpdateAsync(Guid userId, Guid budgetId, BudgetUpdateDto dto);
        Task DeleteAsync(Guid id, Guid userId);
    }

}
