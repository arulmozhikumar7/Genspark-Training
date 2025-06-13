using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace ExpenseTrackerAPI.Hubs
{
    public class BudgetHub : Hub
    {
        public async Task JoinUserGroup(string userId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, userId);
        }

        public async Task LeaveUserGroup(string userId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, userId);
        }
    }
}
