using ExpenseTrackerAPI.Data;
using ExpenseTrackerAPI.Models;
using ExpenseTrackerAPI.Repositories;
using ExpenseTrackerAPI.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ExpenseTrackerAPI.Tests.Repositories
{
    public class ExpenseRepositoryTests
    {
        private async Task<ExpenseTrackerDbContext> GetDbContextAsync()
        {
            var options = new DbContextOptionsBuilder<ExpenseTrackerDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new ExpenseTrackerDbContext(options);

            var userId = Guid.NewGuid();
            var category = new Category { Id = Guid.NewGuid(), Name = "Food" };

            await context.Categories.AddAsync(category);

            var expense = new Expense
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                CategoryId = category.Id,
                Category = category,
                Amount = 100,
                Description = "Lunch",
                ExpenseDate = DateTime.UtcNow
            };

            await context.Expenses.AddAsync(expense);
            await context.SaveChangesAsync();

            return context;
        }

        [Fact]
        public async Task GetAllByUserIdAsync_ShouldReturnExpenses()
        {
            var context = await GetDbContextAsync();
            var repo = new ExpenseRepository(context);

            var userId = context.Expenses.First().UserId!.Value;

            var result = await repo.GetAllByUserIdAsync(userId);

            Assert.Single(result);
            Assert.Equal("Lunch", result.First().Description);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnExpense()
        {
            var context = await GetDbContextAsync();
            var repo = new ExpenseRepository(context);

            var expense = context.Expenses.First();

            var result = await repo.GetByIdAsync(expense.Id, expense.UserId!.Value);

            Assert.NotNull(result);
            Assert.Equal(expense.Amount, result!.Amount);
        }

        [Fact]
        public async Task AddAsync_ShouldAddNewExpense()
        {
            var context = await GetDbContextAsync();
            var repo = new ExpenseRepository(context);

            var userId = context.Expenses.First().UserId!.Value;
            var categoryId = context.Categories.First().Id;

            var newExpense = new Expense
            {
                UserId = userId,
                CategoryId = categoryId,
                Amount = 200,
                Description = "Dinner",
                ExpenseDate = DateTime.UtcNow
            };

            await repo.AddAsync(newExpense);

            Assert.Equal(2, await context.Expenses.CountAsync());
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateExpense()
        {
            var context = await GetDbContextAsync();
            var repo = new ExpenseRepository(context);

            var expense = context.Expenses.First();
            expense.Description = "Updated Description";

            await repo.UpdateAsync(expense);

            var updated = await context.Expenses.FindAsync(expense.Id);
            Assert.Equal("Updated Description", updated!.Description);
        }

        [Fact]
        public async Task DeleteAsync_ShouldSoftDeleteExpense()
        {
            var context = await GetDbContextAsync();
            var repo = new ExpenseRepository(context);

            var expense = context.Expenses.First();

            await repo.DeleteAsync(expense.Id, expense.UserId!.Value);

            var deleted = await context.Expenses.FindAsync(expense.Id);
            Assert.True(deleted!.IsDeleted);
        }

        [Fact]
        public async Task GetFilteredAsync_ShouldApplyFiltersAndReturnCorrectData()
        {
            var context = await GetDbContextAsync();
            var repo = new ExpenseRepository(context);
            var userId = context.Expenses.First().UserId!.Value;

            var parameters = new ExpenseQueryParameters
            {
                CategoryId = context.Categories.First().Id,
                MinAmount = 50,
                MaxAmount = 150,
                SortBy = "amount",
                SortDirection = "desc",
                Page = 1,
                PageSize = 10
            };

            var (expenses, total) = await repo.GetFilteredAsync(userId, parameters);

            Assert.Single(expenses);
            Assert.Equal(1, total);
            Assert.Equal("Lunch", expenses.First().Description);
        }
    }
}
