using HospitalManagementAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HospitalManagementAPI.Interfaces
{
    public interface IPatientRepository
    {
        Task<Patient?> GetByIdAsync(int id);
        Task<IEnumerable<Patient>> GetAllActiveAsync();
        Task AddAsync(Patient patient);
        Task UpdateAsync(Patient patient);
        Task<bool> ExistsAsync(int id);
    }
}
