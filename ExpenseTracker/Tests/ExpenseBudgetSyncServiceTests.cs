using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExpenseTrackerAPI.Data;
using ExpenseTrackerAPI.Interfaces;
using ExpenseTrackerAPI.Models;
using ExpenseTrackerAPI.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace ExpenseTrackerAPI.Tests.Services
{
    public class ExpenseBudgetSyncServiceTests
    {
        private readonly ExpenseTrackerDbContext _context;
        private readonly Mock<IBudgetAlertService> _budgetAlertServiceMock;
        private readonly ExpenseBudgetSyncService _service;

        public ExpenseBudgetSyncServiceTests()
        {
            var options = new DbContextOptionsBuilder<ExpenseTrackerDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ExpenseTrackerDbContext(options);
            _budgetAlertServiceMock = new Mock<IBudgetAlertService>();
            _service = new ExpenseBudgetSyncService(_context, _budgetAlertServiceMock.Object);

            _context.Database.EnsureCreated();
        }

        [Fact]
        public async Task SyncBudgetsForUserAsync_UpdatesBalanceAndTriggersAlert()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var categoryId = Guid.NewGuid();

            var category = new Category { Id = categoryId, Name = "Food" };
            var budget = new Budget
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                CategoryId = categoryId,
                Category = category,
                LimitAmount = 500,
                BalanceAmount = 500,
                StartDate = DateTime.UtcNow.AddDays(-10),
                EndDate = DateTime.UtcNow.AddDays(10),
                IsDeleted = false
            };

            var expense1 = new Expense
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                CategoryId = categoryId,
                ExpenseDate = DateTime.UtcNow,
                Amount = 100,
                IsDeleted = false
            };

            var expense2 = new Expense
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                CategoryId = categoryId,
                ExpenseDate = DateTime.UtcNow,
                Amount = 50,
                IsDeleted = false
            };

            await _context.Categories.AddAsync(category);
            await _context.Budgets.AddAsync(budget);
            await _context.Expenses.AddRangeAsync(expense1, expense2);
            await _context.SaveChangesAsync();

            // Act
            await _service.SyncBudgetsForUserAsync(userId);

            // Assert
            var updatedBudget = await _context.Budgets.FindAsync(budget.Id);
            Assert.Equal(350, updatedBudget!.BalanceAmount);

            _budgetAlertServiceMock.Verify(b => b.CheckAndNotifyAsync(It.IsAny<Budget>()), Times.Once);
        }

        [Fact]
        public async Task SyncBudgetsForUserAsync_IgnoresDeletedBudgetsAndExpenses()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var categoryId = Guid.NewGuid();

            var category = new Category { Id = categoryId, Name = "Food" };

            var budget = new Budget
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                CategoryId = categoryId,
                Category = category,
                LimitAmount = 200,
                BalanceAmount = 200,
                StartDate = DateTime.UtcNow.AddDays(-5),
                EndDate = DateTime.UtcNow.AddDays(5),
                IsDeleted = true
            };

            var expense = new Expense
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                CategoryId = categoryId,
                ExpenseDate = DateTime.UtcNow,
                Amount = 100,
                IsDeleted = true
            };

            await _context.Categories.AddAsync(category);
            await _context.Budgets.AddAsync(budget);
            await _context.Expenses.AddAsync(expense);
            await _context.SaveChangesAsync();

            // Act
            await _service.SyncBudgetsForUserAsync(userId);

            // Assert
            var reloaded = await _context.Budgets.FindAsync(budget.Id);
            Assert.Equal(200, reloaded!.BalanceAmount); // unchanged
            _budgetAlertServiceMock.Verify(b => b.CheckAndNotifyAsync(It.IsAny<Budget>()), Times.Never);
        }
    }
}
