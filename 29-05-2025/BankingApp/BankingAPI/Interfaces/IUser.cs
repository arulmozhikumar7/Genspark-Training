using System.Threading.Tasks;
using BankingAPI.Models;

namespace BankingAPI.Interfaces
{
    public interface IUser
    {
        Task<User> AddUserAsync(User user);
    }
}
