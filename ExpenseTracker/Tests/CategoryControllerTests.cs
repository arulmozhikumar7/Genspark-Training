using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using ExpenseTrackerAPI.Controllers;
using ExpenseTrackerAPI.DTOs;
using ExpenseTrackerAPI.Interfaces;
using ExpenseTrackerAPI.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpenseTracker.Tests.Controllers
{
    public class CategoryControllerTests
    {
        private readonly Mock<ICategoryService> _mockService;
        private readonly CategoryController _controller;

        public CategoryControllerTests()
        {
            _mockService = new Mock<ICategoryService>();
            _controller = new CategoryController(_mockService.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOkWithCategories()
        {
            var categories = new List<CategoryReadDto> {
                new CategoryReadDto { Id = Guid.NewGuid(), Name = "Food" }
            };
            _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(categories);

            var result = await _controller.GetAll();
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var response = okResult.Value.Should().BeAssignableTo<ApiResponse<IEnumerable<CategoryReadDto>>>().Subject;
            response.Data.Should().BeEquivalentTo(categories);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenCategoryNotExists()
        {
            _mockService.Setup(s => s.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((CategoryReadDto?)null);

            var result = await _controller.GetById(Guid.NewGuid());

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task GetById_ReturnsOk_WhenCategoryExists()
        {
            var category = new CategoryReadDto { Id = Guid.NewGuid(), Name = "Utilities" };
            _mockService.Setup(s => s.GetByIdAsync(category.Id)).ReturnsAsync(category);

            var result = await _controller.GetById(category.Id);

            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var response = okResult.Value.Should().BeAssignableTo<ApiResponse<CategoryReadDto>>().Subject;
            response.Data.Should().BeEquivalentTo(category);
        }

        [Fact]
        public async Task Create_ReturnsCreated_WhenValid()
        {
            var dto = new CategoryCreateDto { Name = "Transport" };
            var created = new CategoryReadDto { Id = Guid.NewGuid(), Name = "Transport" };

            _mockService.Setup(s => s.CreateAsync(dto)).ReturnsAsync(created);

            var result = await _controller.Create(dto);

            var createdResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
            var response = createdResult.Value.Should().BeAssignableTo<ApiResponse<CategoryReadDto>>().Subject;
            response.Data.Should().BeEquivalentTo(created);
        }

        [Fact]
        public async Task Create_ReturnsConflict_WhenCategoryExists()
        {
            var dto = new CategoryCreateDto { Name = "Food" };

            _mockService.Setup(s => s.CreateAsync(dto)).ThrowsAsync(new Exception("Category name already exists"));

            var result = await _controller.Create(dto);

            result.Should().BeOfType<ConflictObjectResult>();
        }

        [Fact]
        public async Task Update_ReturnsNotFound_WhenCategoryNotExists()
        {
            var id = Guid.NewGuid();
            var dto = new CategoryUpdateDto { Name = "Updated" };

            _mockService.Setup(s => s.UpdateAsync(id, dto)).ReturnsAsync(false);

            var result = await _controller.Update(id, dto);

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task Update_ReturnsOk_WhenSuccessful()
        {
            var id = Guid.NewGuid();
            var dto = new CategoryUpdateDto { Name = "Updated" };

            _mockService.Setup(s => s.UpdateAsync(id, dto)).ReturnsAsync(true);

            var result = await _controller.Update(id, dto);

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task Update_ReturnsConflict_WhenDuplicateName()
        {
            var id = Guid.NewGuid();
            var dto = new CategoryUpdateDto { Name = "Food" };

            _mockService.Setup(s => s.UpdateAsync(id, dto)).ThrowsAsync(new Exception("Category name already exists"));

            var result = await _controller.Update(id, dto);

            result.Should().BeOfType<ConflictObjectResult>();
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenCategoryNotExists()
        {
            _mockService.Setup(s => s.DeleteAsync(It.IsAny<Guid>())).ReturnsAsync(false);

            var result = await _controller.Delete(Guid.NewGuid());

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task Delete_ReturnsOk_WhenDeleted()
        {
            _mockService.Setup(s => s.DeleteAsync(It.IsAny<Guid>())).ReturnsAsync(true);

            var result = await _controller.Delete(Guid.NewGuid());

            result.Should().BeOfType<OkObjectResult>();
        }
    }
}
