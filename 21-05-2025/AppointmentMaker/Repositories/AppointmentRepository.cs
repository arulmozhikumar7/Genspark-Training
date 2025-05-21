namespace AppointmentMaker.Repository;

using AppointmentMaker.Models;
using AppointmentMaker.Interfaces;

public class AppointmentRepository : IAppointmentRepository
{
    private readonly List<Appointment> _appointments = new();

    public void Add(Appointment appointment)
    {
        appointment.Id = GenerateID();
        _appointments.Add(appointment);
    }

    public List<Appointment> GetAll() => _appointments;

    public List<Appointment> FindByName(string name)
    {
        return _appointments
            .Where(a => a.PatientName.Contains(name, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    public List<Appointment> FindByDate(DateTime date)
    {
        return _appointments
            .Where(a => a.AppointmentDate.Date == date.Date)
            .ToList();
    }

    public List<Appointment> FindByAge(int age)
    {
        return _appointments
            .Where(a => a.Age == age)
            .ToList();
    }

    //  Custom ID generator
    private int GenerateID()
    {
        return _appointments.Count == 0 ? 101 : _appointments.Max(a => a.Id) + 1;
    }

    public IEnumerable<Appointment> SearchAppointments(string? name, DateTime? date, int? minAge, int? maxAge)
    {
        return _appointments.Where(a =>
            (string.IsNullOrEmpty(name) || a.PatientName.Contains(name, StringComparison.OrdinalIgnoreCase)) &&
            (!date.HasValue || a.AppointmentDate.Date == date.Value.Date) &&
            (!minAge.HasValue || a.Age >= minAge.Value) &&
            (!maxAge.HasValue || a.Age <= maxAge.Value)
        );
    }

}
