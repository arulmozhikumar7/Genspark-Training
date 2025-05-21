using AppointmentMaker.Models;

namespace AppointmentMaker.Interfaces;

public interface IAppointmentRepository
{
    void Add(Appointment appointment);
    List<Appointment> GetAll();
    List<Appointment> FindByName(string name);
    List<Appointment> FindByDate(DateTime date);
    List<Appointment> FindByAge(int age);

    IEnumerable<Appointment> SearchAppointments(string? name, DateTime? date, int? minAge, int? maxAge);

}
