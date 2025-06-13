using ExpenseTrackerAPI.Interfaces;
using ExpenseTrackerAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using ExpenseTrackerAPI.Data; 

namespace ExpenseTrackerAPI.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly ExpenseTrackerDbContext _context;

        public RefreshTokenRepository(ExpenseTrackerDbContext context)
        {
            _context = context;
        }

        public async Task<RefreshToken?> GetRefreshTokenAsync(Guid userId)
        {
            var now = DateTime.UtcNow;

            var token = await _context.RefreshTokens
                .Where(r => r.UserId == userId && r.RevokedAt == null && r.ExpiresAt > now)
                .FirstOrDefaultAsync();
            return token;
        }


        public async Task SaveRefreshTokenAsync(Guid userId, RefreshToken refreshToken)
        {
            refreshToken.UserId = userId;

            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRefreshTokenAsync(Guid userId, RefreshToken refreshToken)
        {
            var now = DateTime.UtcNow;

            var existingToken = await _context.RefreshTokens
                .Where(r => r.UserId == userId && r.RevokedAt == null && r.ExpiresAt > now)
                .FirstOrDefaultAsync();

            if (existingToken != null)
            {
                existingToken.RevokedAt = now;
                existingToken.RevokedByIp = refreshToken.CreatedByIp;
                existingToken.ReplacedByToken = refreshToken.Token;
            }

            refreshToken.UserId = userId;

            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();
        }
        
        public async Task RevokeTokenAsync(RefreshToken token)
        {
            _context.RefreshTokens.Update(token);
            await _context.SaveChangesAsync();
        }

    }
}
