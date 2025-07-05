using ExpenseTrackerAPI.Data;
using ExpenseTrackerAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace ExpenseTrackerAPI.Repositories
{
    public interface ITourProgressRepository
    {
        Task<TourProgress> GetOrCreateAsync(Guid userId);
        Task<TourProgress?> GetByUserIdAsync(Guid userId);
        Task<TourProgress> UpdateTourCompletionAsync(Guid userId, string tourKey);
    }

    public class TourProgressRepository : ITourProgressRepository
    {
        private readonly ExpenseTrackerDbContext _context;

        public TourProgressRepository(ExpenseTrackerDbContext context)
        {
            _context = context;
        }

        public async Task<TourProgress> GetOrCreateAsync(Guid userId)
        {
            var progress = await _context.TourProgress
                .FirstOrDefaultAsync(tp => tp.UserId == userId);

            if (progress == null)
            {
                progress = new TourProgress
                {
                    Id = Guid.NewGuid(),
                    UserId = userId
                };

                _context.TourProgress.Add(progress);
                await _context.SaveChangesAsync();
            }

            return progress;
        }

        public async Task<TourProgress?> GetByUserIdAsync(Guid userId)
        {
            return await _context.TourProgress
                .AsNoTracking()
                .FirstOrDefaultAsync(tp => tp.UserId == userId);
        }

        public async Task<TourProgress> UpdateTourCompletionAsync(Guid userId, string tourKey)
        {
            var progress = await GetOrCreateAsync(userId);

            switch (tourKey)
            {
                case "homeTourCompleted":
                    progress.HomeTourCompleted = true;
                    break;
                case "budgetTourCompleted":
                    progress.BudgetTourCompleted = true;
                    break;
                case "expenseTourCompleted":
                    progress.ExpenseTourCompleted = true;
                    break;
                case "reportTourCompleted":
                    progress.ReportTourCompleted = true;
                    break;
                default:
                    throw new ArgumentException("Invalid tour key. Must be one of: home, budget, expense, report");
            }

            progress.UpdatedAt = DateTime.UtcNow;

            _context.TourProgress.Update(progress);
            await _context.SaveChangesAsync();

            return progress;
        }
    }
}
