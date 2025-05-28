using HospitalManagementAPI.Models;
using HospitalManagementAPI.Services;
using HospitalManagementAPI.Dtos;
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

        [HttpPost]
        public async Task<IActionResult> CreateDoctor([FromBody] DoctorCreateDto dto)
        {
             if (!ModelState.IsValid)
                 return BadRequest(ModelState);

            var specialization = await _specializationService.GetSpecializationByIdAsync(dto.SpecializationId);
            if (specialization == null)
                return BadRequest($"Specialization with ID {dto.SpecializationId} does not exist.");

            var doctor = new Doctor
            {
                Name = dto.Name,
                SpecializationId = dto.SpecializationId
            };
            
            await _doctorService.AddDoctorAsync(doctor); 

            var createdDoctor = await _doctorService.GetDoctorByIdAsync(doctor.Id);
            if (createdDoctor == null)
                return StatusCode(500, "Doctor creation failed unexpectedly.");
            var readDto = new DoctorReadDto
            {
                Id = createdDoctor.Id,
                Name = createdDoctor.Name,
                SpecializationId = createdDoctor.SpecializationId,
                IsActive = createdDoctor.IsActive,
                SpecializationName = createdDoctor.Specialization?.Name
            };

            return CreatedAtAction(nameof(GetDoctor), new { id = doctor.Id }, readDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDoctor(int id)
        {
            var doctor = await _doctorService.GetDoctorByIdAsync(id);
            if (doctor == null) return NotFound();

            var dto = new DoctorReadDto
            {
                Id = doctor.Id,
                Name = doctor.Name,
                SpecializationId = doctor.SpecializationId,
                IsActive = doctor.IsActive,
                SpecializationName = doctor.Specialization?.Name
            };

            return Ok(dto);
        }
        
    }
}