using HospitalManagementAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HospitalManagementAPI.Interfaces
{
    public interface ISpecializationRepository
    {
        Task<Specialization?> GetByIdAsync(int id);
        Task<IEnumerable<Specialization>> GetAllAsync();
        Task AddAsync(Specialization specialization);
        Task UpdateAsync(Specialization specialization);
    }
}
