using ExpenseTrackerAPI.DTOs;
using ExpenseTrackerAPI.Models;
using System.Security.Claims;

namespace ExpenseTrackerAPI.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse?> AuthenticateAsync(LoginRequest request);
        Task<AuthResponse?> RefreshAccessTokenAsync(string token, string refreshToken);
        ClaimsPrincipal? ValidateToken(string token);
        Task LogoutAsync(Guid userId, string refreshToken);

    }
}
