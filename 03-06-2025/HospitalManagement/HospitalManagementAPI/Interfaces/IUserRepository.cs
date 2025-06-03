using HospitalManagementAPI.Models;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(int id);
    Task UpdateAsync(User user);
    Task SaveChangesAsync();
    Task<Doctor?> GetDoctorByUserIdAsync(int userId);
}
