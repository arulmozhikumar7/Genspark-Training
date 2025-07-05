using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExpenseTrackerAPI.Services;
using ExpenseTrackerAPI.DTOs;
using ExpenseTrackerAPI.Interfaces;
using ExpenseTrackerAPI.Models;

namespace ExpenseTrackerAPI.Tests.Services
{
    public class CategoryServiceTests
    {
        private readonly Mock<ICategoryRepository> _repoMock;
        private readonly CategoryService _service;

        public CategoryServiceTests()
        {
            _repoMock = new Mock<ICategoryRepository>();
            _service = new CategoryService(_repoMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllCategories()
        {
            // Arrange
            var categories = new List<Category>
            {
                new Category { Id = Guid.NewGuid(), Name = "Food" },
                new Category { Id = Guid.NewGuid(), Name = "Transport" }
            };

            _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(categories);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Contains(result, c => c.Name == "Food");
        }

        [Fact]
        public async Task GetByIdAsync_ExistingId_ReturnsCategory()
        {
            var id = Guid.NewGuid();
            _repoMock.Setup(r => r.GetByIdAsync(id))
                     .ReturnsAsync(new Category { Id = id, Name = "Health" });

            var result = await _service.GetByIdAsync(id);

            Assert.NotNull(result);
            Assert.Equal("Health", result!.Name);
        }

        [Fact]
        public async Task GetByIdAsync_NonExistingId_ReturnsNull()
        {
            _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                     .ReturnsAsync((Category?)null);

            var result = await _service.GetByIdAsync(Guid.NewGuid());

            Assert.Null(result);
        }

        [Fact]
        public async Task CreateAsync_NewCategory_Succeeds()
        {
            var dto = new CategoryCreateDto { Name = "NewCategory" };

            _repoMock.Setup(r => r.ExistsByNameAsync(dto.Name, null)).ReturnsAsync(false);

            Category? captured = null;
            _repoMock.Setup(r => r.AddAsync(It.IsAny<Category>()))
                     .Callback<Category>(c => captured = c)
                     .Returns(Task.CompletedTask);

            _repoMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(true);

            var result = await _service.CreateAsync(dto);

            Assert.NotNull(result);
            Assert.Equal(dto.Name, result.Name);
            Assert.Equal(dto.Name, captured!.Name);
        }

        [Fact]
        public async Task CreateAsync_DuplicateName_ThrowsException()
        {
            var dto = new CategoryCreateDto { Name = "Existing" };
            _repoMock.Setup(r => r.ExistsByNameAsync(dto.Name, null)).ReturnsAsync(true);

            await Assert.ThrowsAsync<Exception>(() => _service.CreateAsync(dto));
        }

        [Fact]
        public async Task UpdateAsync_ValidUpdate_ReturnsTrue()
        {
            var id = Guid.NewGuid();
            var existing = new Category { Id = id, Name = "OldName" };
            var dto = new CategoryUpdateDto { Name = "UpdatedName" };

            _repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(existing);
            _repoMock.Setup(r => r.ExistsByNameAsync(dto.Name, id)).ReturnsAsync(false);
            _repoMock.Setup(r => r.UpdateAsync(existing)).Returns(Task.CompletedTask);
            _repoMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(true);

            var result = await _service.UpdateAsync(id, dto);

            Assert.True(result);
            Assert.Equal("UpdatedName", existing.Name);
        }

        [Fact]
        public async Task UpdateAsync_CategoryNotFound_ReturnsFalse()
        {
            _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Category?)null);

            var result = await _service.UpdateAsync(Guid.NewGuid(), new CategoryUpdateDto { Name = "New" });

            Assert.False(result);
        }

        [Fact]
        public async Task UpdateAsync_DuplicateName_ThrowsException()
        {
            var id = Guid.NewGuid();
            var dto = new CategoryUpdateDto { Name = "Duplicate" };
            var existing = new Category { Id = id, Name = "OldName" };

            _repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(existing);
            _repoMock.Setup(r => r.ExistsByNameAsync(dto.Name, id)).ReturnsAsync(true);

            await Assert.ThrowsAsync<Exception>(() => _service.UpdateAsync(id, dto));
        }

        [Fact]
        public async Task DeleteAsync_ExistingId_ReturnsTrue()
        {
            var id = Guid.NewGuid();
            var category = new Category { Id = id };

            _repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(category);
            _repoMock.Setup(r => r.DeleteAsync(category)).Returns(Task.CompletedTask);
            _repoMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(true);

            var result = await _service.DeleteAsync(id);

            Assert.True(result);
        }

        [Fact]
        public async Task DeleteAsync_NotFound_ReturnsFalse()
        {
            _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Category?)null);

            var result = await _service.DeleteAsync(Guid.NewGuid());

            Assert.False(result);
        }
    }
}
