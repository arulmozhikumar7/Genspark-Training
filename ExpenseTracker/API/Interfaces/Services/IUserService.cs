using ExpenseTrackerAPI.Models;

namespace ExpenseTrackerAPI.Interfaces
{
    public interface IUserService
    {
        Task<User?> GetUserByIdAsync(Guid id);
        Task<User?> GetUserByEmailAsync(string email);
        Task<bool> RegisterUserAsync(User newUser, string password);
        Task<bool> IsEmailTakenAsync(string email);
    }
}
