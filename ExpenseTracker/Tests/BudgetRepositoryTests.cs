using ExpenseTrackerAPI.Data;
using ExpenseTrackerAPI.Models;
using ExpenseTrackerAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ExpenseTrackerAPI.Tests.Repositories
{
    public class BudgetRepositoryTests
    {
        private async Task<ExpenseTrackerDbContext> GetDbContextWithDataAsync()
        {
            var options = new DbContextOptionsBuilder<ExpenseTrackerDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new ExpenseTrackerDbContext(options);

            var userId = Guid.NewGuid();
            var category = new Category { Id = Guid.NewGuid(), Name = "Food" };

            var budgets = new List<Budget>
            {
                new Budget
                {
                    Id = Guid.NewGuid(),
                    Name = "Food Budget",
                    UserId = userId,
                    CategoryId = category.Id,
                    Category = category,
                    LimitAmount = 1000m,
                    BalanceAmount = 200m,
                    StartDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow.AddMonths(1),
                    IsDeleted = false
                },
                new Budget
                {
                    Id = Guid.NewGuid(),
                    Name = "Deleted Budget",
                    UserId = userId,
                    CategoryId = category.Id,
                    Category = category,
                    LimitAmount = 2000m,
                    BalanceAmount = 500m,
                    StartDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow.AddMonths(1),
                    IsDeleted = true // should not show up
                }
            };

            await context.Categories.AddAsync(category);
            await context.Budgets.AddRangeAsync(budgets);
            await context.SaveChangesAsync();

            return context;
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnOnlyActiveBudgets()
        {
            var context = await GetDbContextWithDataAsync();
            var repo = new BudgetRepository(context);
            var userId = context.Budgets.First().UserId!.Value;

            var results = await repo.GetAllAsync(userId);

            Assert.Single(results);
            Assert.Equal("Food Budget", results.First().Name);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnBudget_WhenExistsAndNotDeleted()
        {
            var context = await GetDbContextWithDataAsync();
            var repo = new BudgetRepository(context);

            var expected = context.Budgets.First(b => !b.IsDeleted);
            var result = await repo.GetByIdAsync(expected.Id, expected.UserId!.Value);

            Assert.NotNull(result);
            Assert.Equal(expected.LimitAmount, result!.LimitAmount);
            Assert.Equal(expected.BalanceAmount, result.BalanceAmount);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_IfBudgetDeleted()
        {
            var context = await GetDbContextWithDataAsync();
            var repo = new BudgetRepository(context);

            var deleted = context.Budgets.First(b => b.IsDeleted);
            var result = await repo.GetByIdAsync(deleted.Id, deleted.UserId!.Value);

            Assert.Null(result);
        }

        [Fact]
        public async Task AddAsync_ShouldAddBudgetSuccessfully()
        {
            var options = new DbContextOptionsBuilder<ExpenseTrackerDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new ExpenseTrackerDbContext(options);
            var repo = new BudgetRepository(context);

            var category = new Category { Id = Guid.NewGuid(), Name = "Transport" };
            await context.Categories.AddAsync(category);

            var newBudget = new Budget
            {
                Name = "Transport Budget",
                UserId = Guid.NewGuid(),
                CategoryId = category.Id,
                LimitAmount = 800m,
                BalanceAmount = 800m,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddMonths(1)
            };

            await repo.AddAsync(newBudget);
            await repo.SaveChangesAsync();

            var saved = await context.Budgets.FirstOrDefaultAsync(b => b.Name == "Transport Budget");
            Assert.NotNull(saved);
            Assert.Equal(800m, saved!.LimitAmount);
        }

        [Fact]
        public async Task UpdateAsync_ShouldModifyBudgetFields()
        {
            var context = await GetDbContextWithDataAsync();
            var repo = new BudgetRepository(context);

            var budget = context.Budgets.First(b => !b.IsDeleted);
            budget.BalanceAmount = 123.45m;

            await repo.UpdateAsync(budget);
            await repo.SaveChangesAsync();

            var updated = await context.Budgets.FindAsync(budget.Id);
            Assert.Equal(123.45m, updated!.BalanceAmount);
        }

        [Fact]
        public async Task DeleteAsync_ShouldSetIsDeletedTrue()
        {
            var context = await GetDbContextWithDataAsync();
            var repo = new BudgetRepository(context);

            var budget = context.Budgets.First(b => !b.IsDeleted);
            await repo.DeleteAsync(budget);
            await repo.SaveChangesAsync();

            var result = await context.Budgets.FindAsync(budget.Id);
            Assert.True(result!.IsDeleted);
        }
    }
}
