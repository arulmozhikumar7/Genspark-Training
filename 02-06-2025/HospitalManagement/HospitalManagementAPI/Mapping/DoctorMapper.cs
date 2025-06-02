using HospitalManagementAPI.Models;
using HospitalManagementAPI.Dtos;

namespace HospitalManagementAPI.Mappers
{
    public static class DoctorMapper
    {
        public static DoctorReadDto ToReadDto(Doctor doctor)
        {
            return new DoctorReadDto
            {
                Id = doctor.Id,
                Name = doctor.Name,
                IsActive = doctor.IsActive,
                SpecializationNames = doctor.DoctorSpecializations?
                    .Where(ds => ds.Specialization != null)
                    .Select(ds => ds.Specialization!.Name)
                    .ToList() ?? new List<string>()
            };
        }

        public static Doctor ToModel(DoctorCreateDto dto)
        {
            return new Doctor
            {
                Name = dto.Name,
                DoctorSpecializations = dto.SpecializationIds
                    .Select(id => new DoctorSpecialization
                    {
                        SpecializationId = id
                    }).ToList()
            };
        }
    }
}
