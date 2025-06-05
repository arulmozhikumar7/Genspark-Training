using NotifyAPI.Models;
namespace NotifyAPI.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}