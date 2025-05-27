using HospitalManagementAPI.Interfaces;
using HospitalManagementAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HospitalManagementAPI.Services
{
    public class DoctorService
    {
        private readonly IDoctorRepository _doctorRepository;

        public DoctorService(IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        public async Task<IEnumerable<Doctor>> GetActiveDoctorsAsync()
        {
            return await _doctorRepository.GetAllActiveAsync();
        }

        public async Task<Doctor?> GetDoctorByIdAsync(int id)
        {
            return await _doctorRepository.GetByIdAsync(id);
        }

        public async Task AddDoctorAsync(Doctor doctor)
        {
            await _doctorRepository.AddAsync(doctor);
        }

        public async Task UpdateDoctorAsync(Doctor doctor)
        {
            await _doctorRepository.UpdateAsync(doctor);
        }

        public async Task<bool> IsDoctorActiveAsync(int doctorId)
        {
            return await _doctorRepository.ExistsAsync(doctorId);
        }
    }
}
