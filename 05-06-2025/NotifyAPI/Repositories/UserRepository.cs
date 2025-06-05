using Microsoft.EntityFrameworkCore;
using NotifyAPI.Data;
using NotifyAPI.Models;
using NotifyAPI.Interfaces;
public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _dbSet.SingleOrDefaultAsync(u => u.Username == username);
    }
}
