using ExpenseTrackerAPI.DTOs;
using ExpenseTrackerAPI.Interfaces;
using ExpenseTrackerAPI.Models;
using ExpenseTrackerAPI.Services;
using ExpenseTrackerAPI.Utils;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTrackerAPI.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly IExpenseRepository _repo;
        private readonly IExpenseBudgetSyncService _budgetSyncService;
        private readonly IReceiptRepository _receiptRepo;

        public ExpenseService(IExpenseRepository repo, IExpenseBudgetSyncService budgetSyncService, IReceiptRepository receiptRepo)
        {
            _repo = repo;
            _budgetSyncService = budgetSyncService;
            _receiptRepo = receiptRepo;
        }

        public async Task<PaginatedResponse<ExpenseResponseDto>> GetAllAsync(Guid userId, int page, int pageSize)
        {
            var expensesQuery = _repo.GetAllByUserIdQueryable(userId);

            var totalCount = await expensesQuery.CountAsync();

            var expenses = await expensesQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Include(e => e.Category)
                .ToListAsync();

            var dtoList = expenses.Select(e => new ExpenseResponseDto
            {
                Id = e.Id,
                Amount = e.Amount,
                CategoryName = e.Category.Name,
                Description = e.Description,
                ExpenseDate = e.ExpenseDate
            });

            return new PaginatedResponse<ExpenseResponseDto>
            {
                Items = dtoList,
                TotalCount = totalCount
            };
        }


        public async Task<ExpenseResponseDto> GetByIdAsync(Guid id, Guid userId)
        {
            var e = await _repo.GetByIdAsync(id, userId)
                ?? throw new KeyNotFoundException("Expense not found");

            return new ExpenseResponseDto
            {
                Id = e.Id,
                Amount = e.Amount,
                CategoryName = e.Category.Name,
                Description = e.Description,
                ExpenseDate = e.ExpenseDate
            };
        }

        public async Task CreateAsync(Guid userId, ExpenseCreateDto dto)
        {
            var expense = new Expense
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                CategoryId = dto.CategoryId,
                Amount = dto.Amount,
                Description = dto.Description,
                ExpenseDate = dto.ExpenseDate
            };

            await _repo.AddAsync(expense);
            await _budgetSyncService.SyncBudgetsForUserAsync(userId);
        }

        public async Task UpdateAsync(Guid userId, Guid expenseId, ExpenseUpdateDto dto)
        {
            var expense = await _repo.GetByIdAsync(expenseId, userId)
       ?? throw new KeyNotFoundException("Expense not found");

            if (dto.CategoryId.HasValue)
                expense.CategoryId = dto.CategoryId.Value;

            if (dto.Amount.HasValue)
                expense.Amount = dto.Amount.Value;

            if (dto.Description != null)
                expense.Description = dto.Description;

            if (dto.ExpenseDate.HasValue)
                expense.ExpenseDate = dto.ExpenseDate.Value;

            await _repo.UpdateAsync(expense);
            await _budgetSyncService.SyncBudgetsForUserAsync(userId);
        }

        public async Task DeleteAsync(Guid id, Guid userId)
        {
            var receipts = await _receiptRepo.GetByExpenseIdAsync(id);
            foreach (var receipt in receipts)
            {

                if (!string.IsNullOrEmpty(receipt.FilePath) && File.Exists(receipt.FilePath))
                {
                    File.Delete(receipt.FilePath);
                }

                await _receiptRepo.DeleteAsync(receipt);
            }
            await _receiptRepo.SaveChangesAsync();
            await _repo.DeleteAsync(id, userId);
            await _budgetSyncService.SyncBudgetsForUserAsync(userId);
        }

        public async Task<PaginatedResponse<ExpenseResponseDto>> GetFilteredAsync(Guid userId, ExpenseQueryParameters parameters)
        {
            var (expenses, totalCount) = await _repo.GetFilteredAsync(userId, parameters);

            var dtoList = expenses.Select(e => new ExpenseResponseDto
            {
                Id = e.Id,
                Amount = e.Amount,
                CategoryName = e.Category.Name,
                Description = e.Description,
                ExpenseDate = e.ExpenseDate
            });

            return new PaginatedResponse<ExpenseResponseDto>
            {
                Items = dtoList,
                TotalCount = totalCount
            };
        }

        public async Task<string> ExportCsvAsync(Guid userId, ExpenseQueryParameters parameters)
        {
            var (expenses, _) = await _repo.GetFilteredAsync(userId, parameters);

            var dtoList = expenses
                .Select(e => new ExpenseCsvDto
                {
                    ExpenseDate = e.ExpenseDate,
                    CategoryName = e.Category.Name,
                    Amount = e.Amount,
                    Description = e.Description
                })
                .OrderBy(e => e.ExpenseDate)
                .ToList();

            var csv = new CsvGenerator().GenerateExpensesCsv(dtoList);
            return csv;
        }

        public async Task<IEnumerable<CategoryExpenseSummaryDto>> GetCategorySummaryAsync(Guid userId, ExpenseQueryParameters parameters)
        {
            return await _repo.GetCategorySummaryAsync(userId, parameters);
        }
        

      public async Task<MonthlyExpenseComparisonSummaryDto> CompareTwoMonthsAsync(Guid userId, int year1, int month1, int year2, int month2)
        {
            var start1 = DateTime.SpecifyKind(new DateTime(year1, month1, 1), DateTimeKind.Utc);
            var end1 = DateTime.SpecifyKind(start1.AddMonths(1).AddDays(-1), DateTimeKind.Utc);
            var start2 = DateTime.SpecifyKind(new DateTime(year2, month2, 1), DateTimeKind.Utc);
            var end2 = DateTime.SpecifyKind(start2.AddMonths(1).AddDays(-1), DateTimeKind.Utc);

            var month1Expenses = await _repo.GetExpensesByDateRangeAsync(userId, start1, end1);
            var month2Expenses = await _repo.GetExpensesByDateRangeAsync(userId, start2, end2);

            var month1Group = month1Expenses
                .GroupBy(e => e.Category.Name)
                .ToDictionary(g => g.Key, g => g.Sum(e => e.Amount));
            var month2Group = month2Expenses
                .GroupBy(e => e.Category.Name)
                .ToDictionary(g => g.Key, g => g.Sum(e => e.Amount));

            var allCategories = month1Group.Keys.Union(month2Group.Keys);
            var comparisons = allCategories
                .Select(cat => new MonthlyCategoryComparisonDto
                {
                    CategoryName = cat,
                    Month1Amount = month1Group.GetValueOrDefault(cat),
                    Month2Amount = month2Group.GetValueOrDefault(cat)
                })
                .OrderByDescending(c => Math.Abs(c.Difference))
                .ToList();

            return new MonthlyExpenseComparisonSummaryDto
            {
                Year1 = year1, Month1 = month1,
                Year2 = year2, Month2 = month2,
                TotalMonth1 = month1Expenses.Sum(e => e.Amount),
                TotalMonth2 = month2Expenses.Sum(e => e.Amount),
                CategoryComparisons = comparisons
            };
        }



    }
}
