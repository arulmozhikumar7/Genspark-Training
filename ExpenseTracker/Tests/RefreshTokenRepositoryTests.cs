using ExpenseTrackerAPI.Data;
using ExpenseTrackerAPI.Models;
using ExpenseTrackerAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ExpenseTrackerAPI.Tests.Repositories
{
    public class RefreshTokenRepositoryTests
    {
        private async Task<ExpenseTrackerDbContext> GetDbContextAsync()
        {
            var options = new DbContextOptionsBuilder<ExpenseTrackerDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new ExpenseTrackerDbContext(options);

            var userId = Guid.NewGuid();
            var token = new RefreshToken
            {
                UserId = userId,
                Token = "existing-token",
                ExpiresAt = DateTime.UtcNow.AddDays(1),
                CreatedByIp = "127.0.0.1"
            };

            await context.RefreshTokens.AddAsync(token);
            await context.SaveChangesAsync();

            return context;
        }

        [Fact]
        public async Task GetRefreshTokenAsync_ShouldReturnActiveToken()
        {
            var context = await GetDbContextAsync();
            var repo = new RefreshTokenRepository(context);

            var userId = context.RefreshTokens.First().UserId!.Value;

            var token = await repo.GetRefreshTokenAsync(userId);

            Assert.NotNull(token);
            Assert.Equal("existing-token", token!.Token);
        }

        [Fact]
        public async Task SaveRefreshTokenAsync_ShouldAddNewToken()
        {
            var options = new DbContextOptionsBuilder<ExpenseTrackerDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var context = new ExpenseTrackerDbContext(options);
            var repo = new RefreshTokenRepository(context);

            var userId = Guid.NewGuid();
            var refreshToken = new RefreshToken
            {
                Token = "new-token",
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                CreatedByIp = "192.168.1.1"
            };

            await repo.SaveRefreshTokenAsync(userId, refreshToken);

            var saved = await context.RefreshTokens.FirstOrDefaultAsync(t => t.Token == "new-token");
            Assert.NotNull(saved);
            Assert.Equal(userId, saved!.UserId);
        }

        [Fact]
        public async Task UpdateRefreshTokenAsync_ShouldRevokeOldTokenAndAddNew()
        {
            var context = await GetDbContextAsync();
            var repo = new RefreshTokenRepository(context);

            var oldToken = context.RefreshTokens.First();
            var userId = oldToken.UserId!.Value;

            var newToken = new RefreshToken
            {
                Token = "new-replacement-token",
                ExpiresAt = DateTime.UtcNow.AddDays(5),
                CreatedByIp = "10.0.0.1"
            };

            await repo.UpdateRefreshTokenAsync(userId, newToken);

            var updatedOld = await context.RefreshTokens.FirstOrDefaultAsync(t => t.Token == oldToken.Token);
            var addedNew = await context.RefreshTokens.FirstOrDefaultAsync(t => t.Token == "new-replacement-token");

            Assert.NotNull(updatedOld!.RevokedAt);
            Assert.Equal("10.0.0.1", updatedOld.RevokedByIp);
            Assert.Equal("new-replacement-token", updatedOld.ReplacedByToken);

            Assert.NotNull(addedNew);
            Assert.Equal(userId, addedNew!.UserId);
        }

        [Fact]
        public async Task RevokeTokenAsync_ShouldUpdateRevokedFields()
        {
            var context = await GetDbContextAsync();
            var repo = new RefreshTokenRepository(context);

            var token = context.RefreshTokens.First();
            token.RevokedAt = DateTime.UtcNow;
            token.RevokedByIp = "203.0.113.5";

            await repo.RevokeTokenAsync(token);

            var result = await context.RefreshTokens.FindAsync(token.Id);
            Assert.NotNull(result!.RevokedAt);
            Assert.Equal("203.0.113.5", result.RevokedByIp);
        }
    }
}
