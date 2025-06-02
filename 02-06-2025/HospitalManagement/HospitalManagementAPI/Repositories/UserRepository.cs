using HospitalManagementAPI.Data;
using HospitalManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

public class UserRepository : IUserRepository
{
    private readonly HospitalDbContext _context;

    public UserRepository(HospitalDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task UpdateAsync(User user)
    {
        _context.Users.Update(user);
        await Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
