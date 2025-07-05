using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ExpenseTrackerAPI.Services;
using ExpenseTrackerAPI.Interfaces;
using ExpenseTrackerAPI.DTOs;
using ExpenseTrackerAPI.Models;

namespace ExpenseTrackerAPI.Tests.Services
{
    public class BudgetServiceTests
    {
        private readonly Mock<IBudgetRepository> _repoMock = new();
        private readonly Mock<IExpenseBudgetSyncService> _syncServiceMock = new();
        private readonly BudgetService _service;

        public BudgetServiceTests()
        {
            _service = new BudgetService(_repoMock.Object, _syncServiceMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsMappedBudgets()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var budgets = new List<Budget>
            {
                new Budget { Id = Guid.NewGuid(), CategoryId = Guid.NewGuid(), LimitAmount = 100, BalanceAmount = 100, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(30), Category = new Category { Name = "Food" } }
            };

            _repoMock.Setup(r => r.GetAllAsync(userId)).ReturnsAsync(budgets);

            // Act
            var result = await _service.GetAllAsync(userId);

            // Assert
            Assert.Single(result);
            Assert.Equal(budgets[0].LimitAmount, result.First().LimitAmount);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsMappedBudget()
        {
            var id = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var budget = new Budget
            {
                Id = id,
                CategoryId = Guid.NewGuid(),
                LimitAmount = 100,
                BalanceAmount = 100,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(30),
                Category = new Category { Name = "Travel" }
            };

            _repoMock.Setup(r => r.GetByIdAsync(id, userId)).ReturnsAsync(budget);

            var result = await _service.GetByIdAsync(id, userId);

            Assert.NotNull(result);
            Assert.Equal("Travel", result?.CategoryName);
        }

        [Fact]
        public async Task CreateAsync_AddsBudgetAndSyncs()
        {
            var userId = Guid.NewGuid();
            var dto = new BudgetCreateDto
            {
                Name = "Monthly Food",
                CategoryId = Guid.NewGuid(),
                LimitAmount = 300,
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddMonths(1)
            };

            // Act
            await _service.CreateAsync(userId, dto);

            // Assert
            _repoMock.Verify(r => r.AddAsync(It.Is<Budget>(b => b.Name == "Monthly Food" && b.UserId == userId)), Times.Once);
            _repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
            _syncServiceMock.Verify(s => s.SyncBudgetsForUserAsync(userId), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesBudgetAndSyncs()
        {
            var userId = Guid.NewGuid();
            var budgetId = Guid.NewGuid();

            var existingBudget = new Budget
            {
                Id = budgetId,
                UserId = userId,
                Name = "Old",
                CategoryId = Guid.NewGuid(),
                LimitAmount = 100,
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(10)
            };

            _repoMock.Setup(r => r.GetByIdAsync(budgetId, userId)).ReturnsAsync(existingBudget);

            var dto = new BudgetUpdateDto
            {
                Name = "Updated",
                LimitAmount = 200
            };

            await _service.UpdateAsync(userId, budgetId, dto);

            _repoMock.Verify(r => r.UpdateAsync(It.Is<Budget>(b => b.Name == "Updated" && b.LimitAmount == 200)), Times.Once);
            _repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
            _syncServiceMock.Verify(s => s.SyncBudgetsForUserAsync(userId), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ThrowsIfBudgetNotFound()
        {
            var userId = Guid.NewGuid();
            var budgetId = Guid.NewGuid();

            _repoMock.Setup(r => r.GetByIdAsync(budgetId, userId)).ReturnsAsync((Budget?)null);

            var dto = new BudgetUpdateDto { Name = "FailCase" };

            await Assert.ThrowsAsync<Exception>(() => _service.UpdateAsync(userId, budgetId, dto));
        }

        [Fact]
        public async Task DeleteAsync_DeletesBudget()
        {
            var userId = Guid.NewGuid();
            var budgetId = Guid.NewGuid();
            var budget = new Budget { Id = budgetId, UserId = userId };

            _repoMock.Setup(r => r.GetByIdAsync(budgetId, userId)).ReturnsAsync(budget);

            await _service.DeleteAsync(budgetId, userId);

            _repoMock.Verify(r => r.DeleteAsync(budget), Times.Once);
            _repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ThrowsIfNotFound()
        {
            var userId = Guid.NewGuid();
            var budgetId = Guid.NewGuid();

            _repoMock.Setup(r => r.GetByIdAsync(budgetId, userId)).ReturnsAsync((Budget?)null);

            await Assert.ThrowsAsync<Exception>(() => _service.DeleteAsync(budgetId, userId));
        }
    }
}
