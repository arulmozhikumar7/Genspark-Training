using ExpenseTrackerAPI.Dtos;
using ExpenseTrackerAPI.Models;
using Microsoft.AspNetCore.SignalR;
using ExpenseTrackerAPI.Hubs;
using ExpenseTrackerAPI.Interfaces;

namespace ExpenseTrackerAPI.Services
{
    public class BudgetAlertService : IBudgetAlertService
    {
        private readonly IHubContext<BudgetHub> _hubContext;

        public BudgetAlertService(IHubContext<BudgetHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task CheckAndNotifyAsync(Budget budget)
        {
            if (budget.IsDeleted || budget.UserId == null) return;

            var used = budget.LimitAmount - budget.BalanceAmount;
            var percentageUsed = (int)((used / budget.LimitAmount) * 100);

            string? message = percentageUsed switch
            {
                >= 100 => "Budget limit exceeded!",
                >= 90 => "90% of budget used.",
                >= 50 => "50% of budget used.",
                _ => null
            };

            if (message != null)
            {
                var alert = new BudgetAlertDto
                {
                    BudgetId = budget.Id,
                    Name = budget.Name,
                    CategoryName = budget.Category.Name,
                    LimitAmount = budget.LimitAmount,
                    UsedAmount = used,
                    UsedPercentage = percentageUsed,
                    Message = message
                };

                await _hubContext.Clients.Group(budget.UserId.ToString()!).SendAsync("BudgetAlert", alert);
            }
        }
    }
}