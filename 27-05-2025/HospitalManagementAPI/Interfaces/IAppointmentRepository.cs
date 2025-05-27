using HospitalManagementAPI.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HospitalManagementAPI.Interfaces
{
    public interface IAppointmentRepository
    {
        Task<Appointment?> GetByIdAsync(int id);
        Task<IEnumerable<Appointment>> GetAppointmentsForDoctorInTimeRangeAsync(int doctorId, DateTime start, DateTime end);
        Task AddAsync(Appointment appointment);
        Task UpdateAsync(Appointment appointment);
    }
}
