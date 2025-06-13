using ExpenseTrackerAPI.Data;
using ExpenseTrackerAPI.Models;
using ExpenseTrackerAPI.Enums;
using ExpenseTrackerAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Xunit;

namespace ExpenseTrackerAPI.Tests.Repositories
{
    public class UserRepositoryTests
    {
        private async Task<ExpenseTrackerDbContext> GetDbContextWithDataAsync()
        {
            var options = new DbContextOptionsBuilder<ExpenseTrackerDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Isolated per test
                .Options;

            var context = new ExpenseTrackerDbContext(options);
            context.Users.AddRange(new List<User>
            {
                new User { Id = Guid.NewGuid(), Email = "user1@example.com", Role = Enums.Roles.User },
                new User { Id = Guid.NewGuid(), Email = "user2@example.com", Role = Enums.Roles.Admin }
            });
            await context.SaveChangesAsync();
            return context;
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllUsers()
        {
            // Arrange
            var context = await GetDbContextWithDataAsync();
            var repository = new UserRepository(context);

            // Act
            var users = await repository.GetAllAsync();

            // Assert
            Assert.Equal(2, await repository.GetAllAsync().ContinueWith(t => t.Result.Count()));
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnCorrectUser()
        {
            // Arrange
            var context = await GetDbContextWithDataAsync();
            var user = await context.Users.FirstAsync();
            var repository = new UserRepository(context);

            // Act
            var result = await repository.GetByIdAsync(user.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(user.Email, result!.Email);
        }

        [Fact]
        public async Task GetByEmailAsync_ShouldReturnCorrectUser()
        {
            // Arrange
            var context = await GetDbContextWithDataAsync();
            var repository = new UserRepository(context);

            // Act
            var result = await repository.GetByEmailAsync("user1@example.com");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("user1@example.com", result!.Email);
        }

        [Fact]
        public async Task ExistsByEmailAsync_ShouldReturnTrue_IfExists()
        {
            // Arrange
            var context = await GetDbContextWithDataAsync();
            var repository = new UserRepository(context);

            // Act
            var exists = await repository.ExistsByEmailAsync("user1@example.com");

            // Assert
            Assert.True(exists);
        }

        [Fact]
        public async Task AddAsync_ShouldAddUser()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ExpenseTrackerDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new ExpenseTrackerDbContext(options);
            var repository = new UserRepository(context);
            var newUser = new User
            {
                Id = Guid.NewGuid(),
                Email = "newuser@example.com",
                Role = Enums.Roles.User
            };

            // Act
            await repository.AddAsync(newUser);
            await repository.SaveChangesAsync();

            // Assert
            var result = await repository.GetByEmailAsync("newuser@example.com");
            Assert.NotNull(result);
        }
    }
}
