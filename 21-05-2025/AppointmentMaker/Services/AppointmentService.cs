namespace AppointmentMaker.Services;

using AppointmentMaker.Models;
using AppointmentMaker.Interfaces;

public class AppointmentService : IAppointmentService
{
    private readonly IAppointmentRepository _repository;

    public AppointmentService(IAppointmentRepository repository)
    {
        _repository = repository;
    }

    public void CreateAppointment(Appointment appointment)
    {
        _repository.Add(appointment);
    }

    public List<Appointment> GetAllAppointments()
    {
        return _repository.GetAll();
    }

    public List<Appointment> FindAppointmentsByName(string name)
    {
        return _repository.FindByName(name);
    }

    public List<Appointment> FindAppointmentsByDate(DateTime date)
    {
        return _repository.FindByDate(date);
    }

    public List<Appointment> FindAppointmentsByAge(int age)
    {
        return _repository.FindByAge(age);
    }

    public IEnumerable<Appointment> SearchAppointments(string? name, DateTime? date, int? minAge, int? maxAge)
    {
        return _repository.SearchAppointments(name, date, minAge, maxAge);
    }
}