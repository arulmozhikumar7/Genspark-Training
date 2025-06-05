using HospitalManagementAPI.Models;

namespace HospitalManagementAPI.Interfaces
{
    public interface IFreeMedicalCampRepository
    {
        Task<FreeMedicalCamp> AddAsync(FreeMedicalCamp camp);
        Task<IEnumerable<FreeMedicalCamp>> GetAllAsync();
        Task<FreeMedicalCamp?> GetByIdAsync(int id);
        Task<IEnumerable<FreeMedicalCamp>> GetByDoctorIdAsync(int doctorId);
        Task<bool> DeleteAsync(int id);
    }
}
