using ExpenseTrackerAPI.DTOs;
using ExpenseTrackerAPI.Interfaces;
using ExpenseTrackerAPI.Models;
using ExpenseTrackerAPI.Utilities;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;

namespace ExpenseTrackerAPI.Services
{
    public class BudgetService : IBudgetService
    {
        private readonly IBudgetRepository _repo;
        private readonly IExpenseBudgetSyncService _budgetSyncService;


        public BudgetService(IBudgetRepository repo, IExpenseBudgetSyncService budgetSyncService)
        {
            _repo = repo;
            _budgetSyncService = budgetSyncService;
        }

        public async Task<PaginatedResponse<BudgetDto>> GetFilteredAsync(Guid userId, BudgetQueryParameters filters)
{
    var (budgets, total) = await _repo.GetFilteredAsync(userId, filters);

    var dtoList = budgets.Select(b => new BudgetDto
    {
        Id = b.Id,
        Name = b.Name,
        CategoryId = b.CategoryId,
        CategoryName = b.Category.Name,
        LimitAmount = b.LimitAmount,
        BalanceAmount = b.BalanceAmount,
        StartDate = b.StartDate,
        EndDate = b.EndDate
    });

    return new PaginatedResponse<BudgetDto>
    {
        Items = dtoList,
        TotalCount = total
    };
}


        public async Task<BudgetDto?> GetByIdAsync(Guid id, Guid userId)
        {
            var b = await _repo.GetByIdAsync(id, userId);
            return b == null ? null : new BudgetDto
            {
                Id = b.Id,
                Name = b.Name,
                CategoryId = b.CategoryId,
                CategoryName = b.Category.Name,
                LimitAmount = b.LimitAmount,
                BalanceAmount = b.BalanceAmount,
                StartDate = b.StartDate,
                EndDate = b.EndDate
            };
        }

        public async Task CreateAsync(Guid userId, BudgetCreateDto dto)
        {
            var budget = new Budget
            {
                UserId = userId,
                Name = dto.Name,
                CategoryId = dto.CategoryId,
                LimitAmount = dto.LimitAmount,
                BalanceAmount = dto.LimitAmount, 
                StartDate = dto.StartDate,
                EndDate = dto.EndDate
            };

            await _repo.AddAsync(budget);
            await _repo.SaveChangesAsync();
            await _budgetSyncService.SyncBudgetsForUserAsync(userId);
        }

        public async Task UpdateAsync(Guid userId, Guid budgetId, BudgetUpdateDto dto)
        {
            var existing = await _repo.GetByIdAsync(budgetId, userId);
            if (existing == null) throw new Exception("Budget not found");

            if (dto.Name != null)
                existing.Name = dto.Name;

            if (dto.CategoryId.HasValue)
                existing.CategoryId = dto.CategoryId.Value;

            if (dto.LimitAmount.HasValue)
                existing.LimitAmount = dto.LimitAmount.Value;

            if (dto.StartDate.HasValue)
                existing.StartDate = dto.StartDate.Value;

            if (dto.EndDate.HasValue)
                existing.EndDate = dto.EndDate.Value;

            await _repo.UpdateAsync(existing);
            await _repo.SaveChangesAsync();
            await _budgetSyncService.SyncBudgetsForUserAsync(userId);
        }


        public async Task DeleteAsync(Guid id, Guid userId)
        {
            var budget = await _repo.GetByIdAsync(id, userId);
            if (budget == null) throw new Exception("Budget not found");

            await _repo.DeleteAsync(budget);
            await _repo.SaveChangesAsync();
        }
    }
}
