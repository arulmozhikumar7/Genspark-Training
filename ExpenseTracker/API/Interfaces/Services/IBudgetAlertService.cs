using ExpenseTrackerAPI.Models;

namespace ExpenseTrackerAPI.Interfaces
{
    public interface IBudgetAlertService
    {
        Task CheckAndNotifyAsync(Budget budget);
    }
}