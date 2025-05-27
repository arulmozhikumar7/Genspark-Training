using HospitalManagementAPI.Models;
using HospitalManagementAPI.Services;
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

        [HttpGet]
        public async Task<IEnumerable<Specialization>> GetAllSpecializations()
        {
            return await _specializationService.GetAllSpecializationsAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Specialization>> GetSpecialization(int id)
        {
            var spec = await _specializationService.GetSpecializationByIdAsync(id);
            if (spec == null) return NotFound();
            return spec;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSpecialization(Specialization specialization)
        {
            await _specializationService.AddSpecializationAsync(specialization);
            return CreatedAtAction(nameof(GetSpecialization), new { id = specialization.Id }, specialization);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSpecialization(int id, Specialization specialization)
        {
            if (id != specialization.Id) return BadRequest();

            var existing = await _specializationService.GetSpecializationByIdAsync(id);
            if (existing == null) return NotFound();

            await _specializationService.UpdateSpecializationAsync(specialization);
            return NoContent();
        }
    }
}
