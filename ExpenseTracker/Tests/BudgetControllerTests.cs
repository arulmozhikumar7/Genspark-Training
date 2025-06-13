using ExpenseTrackerAPI.Controllers;
using ExpenseTrackerAPI.DTOs;
using ExpenseTrackerAPI.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace ExpenseTracker.Tests
{
    public class BudgetControllerTests
    {
        private readonly Mock<IBudgetService> _mockService;
        private readonly BudgetController _controller;
        private readonly Guid _userId;

        public BudgetControllerTests()
        {
            _mockService = new Mock<IBudgetService>();
            _controller = new BudgetController(_mockService.Object);

            _userId = Guid.NewGuid();

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", _userId.ToString())
            }, "mock"));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }

        [Fact]
        public async Task GetAll_ReturnsOkWithBudgets()
        {
            var budgets = new List<BudgetDto>
            {
                new BudgetDto
                {
                    Id = Guid.NewGuid(),
                    CategoryName = "Food",
                    LimitAmount = 500
                }
            };

            _mockService.Setup(s => s.GetAllAsync(_userId)).ReturnsAsync(budgets);

            var result = await _controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
        }

        [Fact]
        public async Task Get_ReturnsOk_WhenBudgetExists()
        {
            var budgetId = Guid.NewGuid();
            var budget = new BudgetDto
            {
                Id = budgetId,
                CategoryName = "Travel",
                LimitAmount = 800
            };

            _mockService.Setup(s => s.GetByIdAsync(budgetId, _userId)).ReturnsAsync(budget);

            var result = await _controller.Get(budgetId);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
        }

        [Fact]
        public async Task Get_ReturnsNotFound_WhenBudgetNotFound()
        {
            var budgetId = Guid.NewGuid();
            _mockService.Setup(s => s.GetByIdAsync(budgetId, _userId)).ThrowsAsync(new KeyNotFoundException());

            var result = await _controller.Get(budgetId);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.NotNull(notFoundResult.Value);
        }

        [Fact]
        public async Task Create_ReturnsOk_WhenValid()
        {
            var dto = new BudgetCreateDto
            {
                CategoryId = Guid.NewGuid(),
                LimitAmount = 1000
            };

            _controller.ModelState.Clear(); // Ensure model state is valid

            _mockService.Setup(s => s.CreateAsync(_userId, dto)).Returns(Task.CompletedTask);

            var result = await _controller.Create(dto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
        }

        [Fact]
        public async Task Update_ReturnsOk_WhenSuccessful()
        {
            var budgetId = Guid.NewGuid();
            var dto = new BudgetUpdateDto
            {
                LimitAmount = 1500
            };

            _controller.ModelState.Clear(); // Ensure model state is valid

            _mockService.Setup(s => s.UpdateAsync(_userId, budgetId, dto)).Returns(Task.CompletedTask);

            var result = await _controller.Update(budgetId, dto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
        }

        [Fact]
        public async Task Update_ReturnsNotFound_WhenBudgetNotFound()
        {
            var budgetId = Guid.NewGuid();
            var dto = new BudgetUpdateDto { LimitAmount = 500 };

            _mockService.Setup(s => s.UpdateAsync(_userId, budgetId, dto)).ThrowsAsync(new KeyNotFoundException());

            var result = await _controller.Update(budgetId, dto);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.NotNull(notFoundResult.Value);
        }

        [Fact]
        public async Task Delete_ReturnsOk_WhenDeleted()
        {
            var budgetId = Guid.NewGuid();

            _mockService.Setup(s => s.DeleteAsync(budgetId, _userId)).Returns(Task.CompletedTask);

            var result = await _controller.Delete(budgetId);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenNotFound()
        {
            var budgetId = Guid.NewGuid();
            _mockService.Setup(s => s.DeleteAsync(budgetId, _userId)).ThrowsAsync(new KeyNotFoundException());

            var result = await _controller.Delete(budgetId);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.NotNull(notFoundResult.Value);
        }
    }
}
