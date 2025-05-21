namespace AppointmentMaker.Models;

public class Appointment
{
    public int Id { get; set; }
    public string PatientName { get; set; } = string.Empty;
    public int Age { get; set; }
    public DateTime AppointmentDate { get; set; }
    public string Reason { get; set; } = string.Empty;

    public override string ToString()
    {
        return $"[{Id}] {PatientName} | Age: {Age} | Date: {AppointmentDate:dd-MM-yyyy} | Reason: {Reason}";
    }
}
