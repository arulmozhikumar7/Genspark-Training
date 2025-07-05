using ExpenseTrackerAPI.DTOs;
using ExpenseTrackerAPI.Utils;
namespace ExpenseTrackerAPI.Interfaces
{
    public interface IExpenseService
    {
        Task<PaginatedResponse<ExpenseResponseDto>> GetAllAsync(Guid userId, int page, int pageSize);
        Task<ExpenseResponseDto> GetByIdAsync(Guid id, Guid userId);
        Task CreateAsync(Guid userId, ExpenseCreateDto dto);
        Task UpdateAsync(Guid userId, Guid expenseId, ExpenseUpdateDto dto);
        Task DeleteAsync(Guid id, Guid userId);
        Task<PaginatedResponse<ExpenseResponseDto>> GetFilteredAsync(Guid userId, ExpenseQueryParameters parameters);
        Task<string> ExportCsvAsync(Guid userId, ExpenseQueryParameters parameters);
        Task<IEnumerable<CategoryExpenseSummaryDto>> GetCategorySummaryAsync(Guid userId, ExpenseQueryParameters parameters);
    }
}