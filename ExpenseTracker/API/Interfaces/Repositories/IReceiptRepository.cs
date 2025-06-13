using ExpenseTrackerAPI.Models;

namespace ExpenseTrackerAPI.Interfaces
{
    public interface IReceiptRepository
    {
        Task<IEnumerable<Receipt>> GetByExpenseIdAsync(Guid expenseId);
        Task<Receipt?> GetByIdAsync(Guid id);
        Task AddAsync(Receipt receipt);
        Task<bool> SaveChangesAsync();
        Task DeleteAsync(Receipt receipt);
    }
}