using System;
using System.Linq;
using System.Threading.Tasks;
using ExpenseTrackerAPI.Data;
using ExpenseTrackerAPI.Services;
using ExpenseTrackerAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTrackerAPI.Services
{
    public class ExpenseBudgetSyncService : IExpenseBudgetSyncService
    {
        private readonly ExpenseTrackerDbContext _context;
        private readonly IBudgetAlertService _budgetAlertService;

        public ExpenseBudgetSyncService(ExpenseTrackerDbContext context, IBudgetAlertService budgetAlertService)
        {
            _context = context;
            _budgetAlertService = budgetAlertService;
        }

        public async Task SyncBudgetsForUserAsync(Guid userId)
        {
            var budgets = await _context.Budgets
                .Include(b => b.Category)
                .Where(b => b.UserId == userId && !b.IsDeleted)
                .ToListAsync();

            var expenses = await _context.Expenses
                .Where(e => e.UserId == userId && !e.IsDeleted)
                .ToListAsync();

            foreach (var budget in budgets)
            {
                var matchingExpenses = expenses
                    .Where(e =>
                        e.ExpenseDate >= budget.StartDate &&
                        e.ExpenseDate <= budget.EndDate &&
                        (budget.Category.Name == "All" || e.CategoryId == budget.CategoryId))
                    .ToList();

                var used = matchingExpenses.Sum(e => e.Amount);
                budget.BalanceAmount = budget.LimitAmount - used;

                _context.Budgets.Update(budget);

                await _budgetAlertService.CheckAndNotifyAsync(budget);
                Console.WriteLine("Called");
            }

            await _context.SaveChangesAsync();
        }
    }
}
