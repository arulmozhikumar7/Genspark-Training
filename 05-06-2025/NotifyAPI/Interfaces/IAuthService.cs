using NotifyAPI.Models;
namespace NotifyAPI.Interfaces
{
    public interface IAuthService
    {
        Task<string?> AuthenticateAsync(string username, string password);
        Task<User> RegisterAsync(User user, string password);
    }
}