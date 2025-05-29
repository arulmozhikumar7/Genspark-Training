using System.Threading.Tasks;
using BankingAPI.Interfaces;
using BankingAPI.Models;
using BankingAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace BankingAPI.Repositories
{
    public class UserRepository : IUser
    {
        private readonly BankingDbContext _context;

        public UserRepository(BankingDbContext context)
        {
            _context = context;
        }

        public async Task<User> AddUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }
    }
}
