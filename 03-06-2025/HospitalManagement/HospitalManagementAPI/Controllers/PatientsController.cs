using HospitalManagementAPI.Models;
using HospitalManagementAPI.Services;
using HospitalManagementAPI.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;


namespace HospitalManagementAPI.Controllers
{
    [ApiController]
    [Route("api/patients")]
    public class PatientController : ControllerBase
    {
        private readonly PatientService _patientService;

        public PatientController(PatientService patientService)
        {
            _patientService = patientService;
        }

        [Authorize(Roles = "patient")]
        [HttpPost]
        public async Task<IActionResult> CreatePatient([FromBody] PatientCreateDto dto)
       {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
                return Unauthorized("Invalid user ID");

            var patient = new Patient
            {
                Name = dto.Name,
                Age = dto.Age,
                Gender = dto.Gender
            };

            try
            {
                var createdPatient = await _patientService.AddPatientAsync(patient, userId);
             return CreatedAtAction(nameof(GetPatient), new { id = createdPatient.Id }, createdPatient);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPatient(int id)
        {
            var patient = await _patientService.GetPatientByIdAsync(id);
            if (patient == null) return NotFound();

            var patientDto = new PatientReadDto
            {
                Id = patient.Id,
                Name = patient.Name,
                Age = patient.Age,
                Gender = patient.Gender,
                IsActive = patient.IsActive
            };

            return Ok(patientDto);
        }
    }
}
