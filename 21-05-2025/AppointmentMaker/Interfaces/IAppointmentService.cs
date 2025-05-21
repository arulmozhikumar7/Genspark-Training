namespace AppointmentMaker.Interfaces;

using AppointmentMaker.Models;

public interface IAppointmentService
{
    void CreateAppointment(Appointment appointment);
    List<Appointment> GetAllAppointments();
    List<Appointment> FindAppointmentsByName(string name);
    List<Appointment> FindAppointmentsByDate(DateTime date);
    List<Appointment> FindAppointmentsByAge(int age);
    IEnumerable<Appointment> SearchAppointments(string? name, DateTime? date, int? minAge, int? maxAge);

}
    