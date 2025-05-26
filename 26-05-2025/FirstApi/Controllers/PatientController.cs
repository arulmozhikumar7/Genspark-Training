using Microsoft.AspNetCore.Mvc;
using FirstApi.Models;
[ApiController]
[Route("api/[controller]")]
public class PatientController : ControllerBase
{
    static List<Patient> patients = new List<Patient>
    {
        new Patient { Id = 1, Name = "Ramu", Age = 30, Disease = "Flu" },
        new Patient { Id = 2, Name = "Somu", Age = 25, Disease = "Cold" }
    };

    // GET All
    [HttpGet]
    public ActionResult GetAll()
    {
        try
        {
            return Ok(patients);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error: {ex.Message}");
        }
    }

    // GET by ID
    [HttpGet("{id}")]
    public ActionResult GetById(int id)
    {
        try
        {
            var patient = patients.FirstOrDefault(p => p.Id == id);
            if (patient == null)
                return NotFound($"Patient with ID {id} not found.");

            return Ok(patient);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error: {ex.Message}");
        }
    }

    // POST
    [HttpPost]
    public ActionResult Create([FromBody] Patient patient)
    {
        try
        {
            int newId = patients.Any() ? patients.Max(p => p.Id) + 1 : 1;
            patient.Id = newId;

            patients.Add(patient);
            return Created("", patient);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error: {ex.Message}");
        }
    }


    // PUT 
    [HttpPut("{id}")]
    public ActionResult Update(int id, [FromBody] Patient updatedPatient)
    {
        try
        {
            var patient = patients.FirstOrDefault(p => p.Id == id);
            if (patient == null)
                return NotFound($"Patient with ID {id} not found.");

            if (!string.IsNullOrWhiteSpace(updatedPatient.Name))
                patient.Name = updatedPatient.Name;

            if (updatedPatient.Age > 0)
                patient.Age = updatedPatient.Age;

            if (!string.IsNullOrWhiteSpace(updatedPatient.Disease))
                patient.Disease = updatedPatient.Disease;

            return Ok(patient);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error: {ex.Message}");
        }
    }

    // DELETE
    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        try
        {
            var patient = patients.FirstOrDefault(p => p.Id == id);
            if (patient == null)
                return NotFound($"Patient with ID {id} not found.");

            patients.Remove(patient);
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error: {ex.Message}");
        }
    }
}
