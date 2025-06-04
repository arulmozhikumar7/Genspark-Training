using HospitalManagementAPI.Interfaces;
using HospitalManagementAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HospitalManagementAPI.Services
{
    public class SpecializationService
    {
        private readonly ISpecializationRepository _specializationRepository;

        public SpecializationService(ISpecializationRepository specializationRepository)
        {
            _specializationRepository = specializationRepository;
        }

        public async Task<IEnumerable<Specialization>> GetAllSpecializationsAsync()
        {
            return await _specializationRepository.GetAllAsync();
        }

        public async Task<Specialization?> GetSpecializationByIdAsync(int id)
        {
            return await _specializationRepository.GetByIdAsync(id);
        }

        public async Task AddSpecializationAsync(Specialization specialization)
        {
            await _specializationRepository.AddAsync(specialization);
        }

       
    }
}
