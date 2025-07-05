using ExpenseTrackerAPI.Models;
using ExpenseTrackerAPI.Repositories;
using ExpenseTrackerAPI.Interfaces;
using System;
using System.Threading.Tasks;


namespace ExpenseTrackerAPI.Services
{
    public class TourProgressService : ITourProgressService
    {
        private readonly ITourProgressRepository _tourProgressRepository;

        public TourProgressService(ITourProgressRepository tourProgressRepository)
        {
            _tourProgressRepository = tourProgressRepository;
        }

        public async Task<TourProgress> GetProgressAsync(Guid userId)
        {
            return await _tourProgressRepository.GetOrCreateAsync(userId);
        }

        public async Task<TourProgress> MarkTourAsCompletedAsync(Guid userId, string tourKey)
        {
            return await _tourProgressRepository.UpdateTourCompletionAsync(userId, tourKey);
        }
    }
}
