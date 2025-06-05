using NotifyAPI.Models;
namespace NotifyAPI.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByUsernameAsync(string username);
    }
}