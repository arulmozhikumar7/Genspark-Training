using Microsoft.AspNetCore.Mvc;
using FirstApi.Models;
[ApiController]
[Route("/api/[controller]")]
public class DoctorController : ControllerBase
{
    static List<Doctor> doctors = new List<Doctor>
    {
        new Doctor { Id = 101, Name = "Ramu" },
        new Doctor { Id = 102, Name = "Somu" },
    };

    [HttpGet]
    public ActionResult<IEnumerable<Doctor>> GetDoctors()
    {
        return Ok(doctors);
    }

    [HttpPost]
    public ActionResult<Doctor> PostDoctor([FromBody] Doctor doctor)
    {
        doctors.Add(doctor);
        return Created("", doctor);
    }

    [HttpPut("{id}")]
    public ActionResult<Doctor> UpdateDoctor(int id, [FromBody] Doctor updatedDoctor)
    {
        var doctor = doctors.FirstOrDefault(d => d.Id == id);
        if (doctor == null)
        {
            return NotFound($"Doctor with ID {id} not found.");
        }

        doctor.Name = updatedDoctor.Name;
        return Ok(doctor);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteDoctor(int id)
    {
        var doctor = doctors.FirstOrDefault(d => d.Id == id);
        if (doctor == null)
        {
            return NotFound($"Doctor with ID {id} not found.");
        }

        doctors.Remove(doctor);
        return NoContent(); 
    }
}
