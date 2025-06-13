using ExpenseTrackerAPI.Models;
using System.Security.Claims;

namespace ExpenseTrackerAPI.Interfaces
{
    public interface ITokenHelper
    {
        string GenerateJwtToken(User user);
        RefreshToken CreateRefreshToken(string createdByIp);
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
    }
}
