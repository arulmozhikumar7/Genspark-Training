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
            string? message = null;

            if (percentageUsed >= 50)
            {
                if (!budget.NotifiedLimitExceeded)
                {
                    message = "50% of budget is used.";
                    budget.NotifiedLimitExceeded = true;
                }
            }
            else
            {
                budget.NotifiedLimitExceeded = false;
            }

            if (percentageUsed >= 90)
            {
                if (!budget.Notified90Percent)
                {
                    message = "90% of budget used.";
                    budget.Notified90Percent = true;
                }
            }
            else
            {
                budget.Notified90Percent = false;
            }

            if (percentageUsed >= 100)
            {
                if (!budget.Notified50Percent)
                {
                    message = "Budget Limit Exceeded";
                    budget.Notified50Percent = true;
                }
            }
            else
            {
                budget.Notified50Percent = false;
            }

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