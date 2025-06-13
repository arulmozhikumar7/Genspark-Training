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
    public class CategoryRepositoryTests
    {
        private async Task<ExpenseTrackerDbContext> GetDbContextWithDataAsync()
        {
            var options = new DbContextOptionsBuilder<ExpenseTrackerDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new ExpenseTrackerDbContext(options);

            var categories = new List<Category>
            {
                new Category
                {
                    Id = Guid.NewGuid(),
                    Name = "Groceries",
                    IsDeleted = false,
                    CreatedAt = DateTime.UtcNow
                },
                new Category
                {
                    Id = Guid.NewGuid(),
                    Name = "Deleted Category",
                    IsDeleted = true,
                    CreatedAt = DateTime.UtcNow
                }
            };

            await context.Categories.AddRangeAsync(categories);
            await context.SaveChangesAsync();

            return context;
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnOnlyActiveCategories()
        {
            var context = await GetDbContextWithDataAsync();
            var repo = new CategoryRepository(context);

            var results = await repo.GetAllAsync();

            Assert.Single(results);
            Assert.Equal("Groceries", results.First().Name);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnCategory_WhenExistsAndNotDeleted()
        {
            var context = await GetDbContextWithDataAsync();
            var repo = new CategoryRepository(context);

            var expected = context.Categories.First(c => !c.IsDeleted);
            var result = await repo.GetByIdAsync(expected.Id);

            Assert.NotNull(result);
            Assert.Equal(expected.Name, result!.Name);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenCategoryIsDeleted()
        {
            var context = await GetDbContextWithDataAsync();
            var repo = new CategoryRepository(context);

            var deleted = context.Categories.First(c => c.IsDeleted);
            var result = await repo.GetByIdAsync(deleted.Id);

            Assert.Null(result);
        }

        [Fact]
        public async Task AddAsync_ShouldAddCategorySuccessfully()
        {
            var options = new DbContextOptionsBuilder<ExpenseTrackerDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new ExpenseTrackerDbContext(options);
            var repo = new CategoryRepository(context);

            var newCategory = new Category
            {
                Name = "Utilities",
                CreatedAt = DateTime.UtcNow
            };

            await repo.AddAsync(newCategory);
            await repo.SaveChangesAsync();

            var saved = await context.Categories.FirstOrDefaultAsync(c => c.Name == "Utilities");
            Assert.NotNull(saved);
            Assert.False(saved!.IsDeleted);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateUpdatedAtTimestamp()
        {
            var context = await GetDbContextWithDataAsync();
            var repo = new CategoryRepository(context);

            var category = context.Categories.First(c => !c.IsDeleted);
            category.UpdatedAt = DateTime.UtcNow;

            await repo.UpdateAsync(category);
            await repo.SaveChangesAsync();

            var updated = await context.Categories.FindAsync(category.Id);
            Assert.NotNull(updated!.UpdatedAt);
        }

        [Fact]
        public async Task DeleteAsync_ShouldMarkCategoryAsDeleted()
        {
            var context = await GetDbContextWithDataAsync();
            var repo = new CategoryRepository(context);

            var category = context.Categories.First(c => !c.IsDeleted);

            await repo.DeleteAsync(category);
            await repo.SaveChangesAsync();

            var result = await context.Categories.FindAsync(category.Id);
            Assert.True(result!.IsDeleted);
        }

        [Fact]
        public async Task ExistsByNameAsync_ShouldReturnTrue_WhenNameExists()
        {
            var context = await GetDbContextWithDataAsync();
            var repo = new CategoryRepository(context);

            var exists = await repo.ExistsByNameAsync("groceries");

            Assert.True(exists);
        }

        [Fact]
        public async Task ExistsByNameAsync_ShouldExcludeCategoryById()
        {
            var context = await GetDbContextWithDataAsync();
            var repo = new CategoryRepository(context);

            var category = context.Categories.First(c => !c.IsDeleted);
            var exists = await repo.ExistsByNameAsync("Groceries", excludeId: category.Id);

            Assert.False(exists); 
        }
    }
}
