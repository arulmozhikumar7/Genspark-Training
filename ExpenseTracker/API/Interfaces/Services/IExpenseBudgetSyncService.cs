using System;
using System.Threading.Tasks;

namespace ExpenseTrackerAPI.Services
{
    public interface IExpenseBudgetSyncService
    {
        Task SyncBudgetsForUserAsync(Guid userId);
    }
}