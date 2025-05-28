using HospitalManagementAPI.Models;
using HospitalManagementAPI.Services;
using HospitalManagementAPI.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HospitalManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SpecializationsController : ControllerBase
    {
        private readonly SpecializationService _specializationService;

        public SpecializationsController(SpecializationService specializationService)
        {
            _specializationService = specializationService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSpecialization([FromBody] SpecializationAddDto specializationDto)
        {
            var specialization = new Specialization
            {
                Name = specializationDto.Name,
                Description = specializationDto.Description,
                IsEnabled = true  
            };

            await _specializationService.AddSpecializationAsync(specialization);

            return CreatedAtAction(nameof(GetSpecialization), new { id = specialization.Id }, specialization);
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<SpecializationDto>>> GetAllSpecializations()
        {
            var specializations = await _specializationService.GetAllSpecializationsAsync();

            var dtos = specializations.Select(s => new SpecializationDto
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                Doctors = s.Doctors?.Select(d => new DoctorDto
                {
                    Id = d.Id,
                    Name = d.Name
                }).ToList()
            });

            return Ok(dtos);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Specialization>> GetSpecialization(int id)
        {
        var specialization = await _specializationService.GetSpecializationByIdAsync(id);
        if (specialization == null)
        {
            return NotFound();
        }

        var dto = new SpecializationDto
        {
            Id = specialization.Id,
            Name = specialization.Name,
            Description = specialization.Description,
            Doctors = specialization.Doctors.Select(d => new DoctorDto
            {
                Id = d.Id,
                Name = d.Name
            }).ToList()
        };

        return Ok(dto);
        }

       
    }
}
