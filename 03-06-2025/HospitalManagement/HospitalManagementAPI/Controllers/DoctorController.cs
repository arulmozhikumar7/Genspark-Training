using HospitalManagementAPI.Models;
using HospitalManagementAPI.Services;
using HospitalManagementAPI.Dtos;
using HospitalManagementAPI.Mappers;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HospitalManagementAPI.Controllers
{
    [ApiController]
    [Route("api/doctors")]
    public class DoctorController : ControllerBase
    {
        private readonly DoctorService _doctorService;
        private readonly SpecializationService _specializationService;

        public DoctorController(DoctorService doctorService, SpecializationService specializationService)
        {
            _doctorService = doctorService;
            _specializationService = specializationService;
        }


        [Authorize(Roles = "doctor")]
        [HttpPost]
        public async Task<IActionResult> CreateDoctor([FromBody] DoctorCreateDto dto)
        {
             if (!ModelState.IsValid)
                return BadRequest(ModelState);

            
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
                return Unauthorized("Invalid user ID");


            var validSpecializations = new List<Specialization>();

            foreach (var specId in dto.SpecializationIds)
            {
                var specialization = await _specializationService.GetSpecializationByIdAsync(specId);
                if (specialization == null)
                    return BadRequest($"Specialization with ID {specId} does not exist.");

                validSpecializations.Add(specialization);
            }
            var doctor = DoctorMapper.ToModel(dto);
            var createdDoctor = await _doctorService.AddDoctorAsync(doctor, userId);
            if (createdDoctor == null)
                return StatusCode(500, "Doctor creation failed unexpectedly.");

            var readDto = DoctorMapper.ToReadDto(createdDoctor);

            return CreatedAtAction(nameof(GetDoctor), new { id = doctor.Id }, readDto);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetDoctor(int id)
        {
            var doctor = await _doctorService.GetDoctorByIdAsync(id);
            if (doctor == null) return NotFound();

            var dto = DoctorMapper.ToReadDto(doctor);

            return Ok(dto);
        }

        
        [HttpPost("bulk")]
        public async Task<IActionResult> BulkCreateDoctors([FromBody] List<DoctorBulkCreateDto> doctors)
        {
            if (doctors == null || !doctors.Any())
                return BadRequest("Input list is empty.");

            try
            {
                await _doctorService.BulkInsertDoctorsAsync(doctors);
                return Ok("Doctors added successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        
    }
}