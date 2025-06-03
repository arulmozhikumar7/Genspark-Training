using HospitalManagementAPI.Data;
using HospitalManagementAPI.Interfaces;
using HospitalManagementAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalManagementAPI.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly HospitalDbContext _context;

        public AppointmentRepository(HospitalDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Appointment appointment)
        {
            await _context.Appointments.AddAsync(appointment);
            await _context.SaveChangesAsync();
        }

        public async Task<Appointment?> GetByIdAsync(int id)
        {
            return await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsForDoctorInTimeRangeAsync(int doctorId, DateTime start, DateTime end)
        {
            var appointments = await _context.Appointments
                .Where(a => a.DoctorId == doctorId)
                .ToListAsync();

            return appointments.Where(a =>
            {
                var startTime = TimeOnly.FromTimeSpan(a.TimePeriod.Start);
                var endTime = TimeOnly.FromTimeSpan(a.TimePeriod.End);

                var appointmentStart = a.Date.ToDateTime(startTime);
                var appointmentEnd = a.Date.ToDateTime(endTime);

                return appointmentStart < end && appointmentEnd > start;
            });

        }


        public async Task UpdateAsync(Appointment appointment)
        {
            _context.Appointments.Update(appointment);
            await _context.SaveChangesAsync();
        }
    }
}
