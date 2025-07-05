using ExpenseTrackerAPI.Models;
using System;
using System.Threading.Tasks;

namespace ExpenseTrackerAPI.Interfaces
{
    public interface ITourProgressService
    {
        Task<TourProgress> GetProgressAsync(Guid userId);
        Task<TourProgress> MarkTourAsCompletedAsync(Guid userId, string tourKey);
    }
}
