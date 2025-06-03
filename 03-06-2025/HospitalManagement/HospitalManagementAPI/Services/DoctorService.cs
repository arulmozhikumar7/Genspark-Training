using HospitalManagementAPI.Interfaces;
using HospitalManagementAPI.Models;
using HospitalManagementAPI.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HospitalManagementAPI.Services
{
    public class DoctorService
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IUserRepository _userRepository;

        public DoctorService(IDoctorRepository doctorRepository, IUserRepository userRepository)
        {
            _doctorRepository = doctorRepository;
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<Doctor>> GetActiveDoctorsAsync()
        {
            return await _doctorRepository.GetAllActiveAsync();
        }

        public async Task<Doctor?> GetDoctorByIdAsync(int id)
        {
            return await _doctorRepository.GetByIdAsync(id);
        }

        public async Task<Doctor> AddDoctorAsync(Doctor doctor, int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new Exception("User not found");

            if (user.DoctorId != null)
                throw new Exception("Doctor already linked to this user");

            await _doctorRepository.AddAsync(doctor);

            user.DoctorId = doctor.Id;
            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();

            return doctor;
        }

        public async Task UpdateDoctorAsync(Doctor doctor)
        {
            await _doctorRepository.UpdateAsync(doctor);
        }

        public async Task<bool> IsDoctorActiveAsync(int doctorId)
        {
            return await _doctorRepository.ExistsAsync(doctorId);
        }

        public async Task BulkInsertDoctorsAsync(List<DoctorBulkCreateDto> doctors)
        {
            await _doctorRepository.BulkInsertDoctorsAsync(doctors);
        }
        
        public async Task<Doctor?> GetDoctorByUserIdAsync(int userId)
        {
            return await _userRepository.GetDoctorByUserIdAsync(userId);
        }


    }
}
