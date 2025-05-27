using HospitalManagementAPI.Interfaces;
using HospitalManagementAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HospitalManagementAPI.Services
{
    public class PatientService
    {
        private readonly IPatientRepository _patientRepository;

        public PatientService(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public async Task<IEnumerable<Patient>> GetActivePatientsAsync()
        {
            return await _patientRepository.GetAllActiveAsync();
        }

        public async Task<Patient?> GetPatientByIdAsync(int id)
        {
            return await _patientRepository.GetByIdAsync(id);
        }

        public async Task AddPatientAsync(Patient patient)
        {
            await _patientRepository.AddAsync(patient);
        }

        public async Task UpdatePatientAsync(Patient patient)
        {
            await _patientRepository.UpdateAsync(patient);
        }

        public async Task<bool> IsPatientActiveAsync(int patientId)
        {
            return await _patientRepository.ExistsAsync(patientId);
        }
    }
}
