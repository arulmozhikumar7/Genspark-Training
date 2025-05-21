// Program.cs

using AppointmentMaker.Helpers;
using AppointmentMaker.Repository;
using AppointmentMaker.Services;
using AppointmentMaker.Interfaces;
using AppointmentMaker;

class Program
{
    static void Main()
    {
        IAppointmentRepository repository = new AppointmentRepository();
        IAppointmentService service = new AppointmentService(repository);
        var manager = new AppointmentManager(service);

        manager.Run();
    }
}
