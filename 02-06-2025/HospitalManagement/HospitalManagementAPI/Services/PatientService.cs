using HospitalManagementAPI.Interfaces;
using HospitalManagementAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HospitalManagementAPI.Services
{
    public class PatientService
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IUserRepository _userRepository;

        public PatientService(IPatientRepository patientRepository, IUserRepository userRepository)
        {
            _patientRepository = patientRepository;
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<Patient>> GetActivePatientsAsync()
        {
            return await _patientRepository.GetAllActiveAsync();
        }

        public async Task<Patient?> GetPatientByIdAsync(int id)
        {
            return await _patientRepository.GetByIdAsync(id);
        }

       public async Task<Patient> AddPatientAsync(Patient patient, int userId)
{
    var user = await _userRepository.GetByIdAsync(userId);
    if (user == null)
        throw new Exception("User not found");

    if (user.PatientId != null)
        throw new Exception("Patient already linked to this user");

    await _patientRepository.AddAsync(patient);

    user.PatientId = patient.Id;
    await _userRepository.UpdateAsync(user);
    await _userRepository.SaveChangesAsync();

    return patient;
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
