using ExpenseTrackerAPI.Models;

namespace ExpenseTrackerAPI.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task SaveRefreshTokenAsync(Guid userId, RefreshToken refreshToken);
        Task<RefreshToken?> GetRefreshTokenAsync(Guid userId);
        Task UpdateRefreshTokenAsync(Guid userId, RefreshToken newToken);
        Task RevokeTokenAsync(RefreshToken token);

    }
}
