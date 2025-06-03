using HospitalManagementAPI.Interfaces;
using HospitalManagementAPI.Models;
using HospitalManagementAPI.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;



public class AppointmentService
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IDoctorRepository _doctorRepository;
    private readonly IPatientRepository _patientRepository;

    public AppointmentService(IAppointmentRepository appointmentRepository,
        IDoctorRepository doctorRepository,
        IPatientRepository patientRepository)
    {
        _appointmentRepository = appointmentRepository;
        _doctorRepository = doctorRepository;
        _patientRepository = patientRepository;
    }

    public async Task<bool> AddAppointmentAsync(Appointment appointment)
    {
        DateTime start = appointment.Date.ToDateTime(TimeOnly.FromTimeSpan(appointment.TimePeriod.Start));
        DateTime end = appointment.Date.ToDateTime(TimeOnly.FromTimeSpan(appointment.TimePeriod.End));
        
        if (start <= DateTime.Now)  
        return false;

        if (start >= end)
            return false;

        var doctor = await _doctorRepository.GetByIdAsync(appointment.DoctorId);
        if (doctor == null || !doctor.IsActive)
            return false;

        var patientExists = await _patientRepository.ExistsAsync(appointment.PatientId);
        if (!patientExists)
            return false;

        var overlapping = await _appointmentRepository.GetAppointmentsForDoctorInTimeRangeAsync(
            appointment.DoctorId, start, end);
        if (overlapping != null && overlapping.Any())
            return false;

        appointment.IsCancelled = false;
        await _appointmentRepository.AddAsync(appointment);
        return true;
    }

   
    public async Task<Appointment?> GetAppointmentByIdAsync(int id)
    {
        return await _appointmentRepository.GetByIdAsync(id);
    }

    public async Task UpdateAppointmentAsync(Appointment appointment)
    {
        await _appointmentRepository.UpdateAsync(appointment);
    }

    public async Task<IEnumerable<AppointmentReadDto>> GetAppointmentsForDoctorAsync(int doctorId)
    {
        var appointments = await _appointmentRepository.GetAppointmentsForDoctorInTimeRangeAsync(
            doctorId, DateTime.MinValue, DateTime.MaxValue);
        var activeAppointments = appointments.Where(a => !a.IsCancelled);
        var appointmentDtos = activeAppointments.Select(a => new AppointmentReadDto
        {
            Id = a.Id,
            PatientId = a.PatientId,
            PatientName = a.Patient?.Name ?? "Unknown",
            DoctorId = a.DoctorId,
            DoctorName = a.Doctor?.Name ?? "Unknown",
            Date = a.Date,
            TimePeriod = a.TimePeriod,
            Reason = a.Reason,
            IsCancelled = a.IsCancelled
        });

        return appointmentDtos;
    }


}
