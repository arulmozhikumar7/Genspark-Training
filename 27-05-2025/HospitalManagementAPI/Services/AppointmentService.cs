using HospitalManagementAPI.Interfaces;
using HospitalManagementAPI.Models;
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

   

    public async Task UpdateAppointmentAsync(Appointment appointment)
    {
        await _appointmentRepository.UpdateAsync(appointment);
    }

    public async Task<IEnumerable<Appointment>> GetAppointmentsForDoctorAsync(int doctorId)
    {
        return await _appointmentRepository.GetAppointmentsForDoctorInTimeRangeAsync(
            doctorId, DateTime.MinValue, DateTime.MaxValue);
    }
}
