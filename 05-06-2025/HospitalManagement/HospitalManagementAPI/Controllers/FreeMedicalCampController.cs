using HospitalManagementAPI.Models;
using HospitalManagementAPI.Services;
using Microsoft.AspNetCore.Mvc;
using HospitalManagementAPI.DTOs;

namespace HospitalManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FreeMedicalCampController : ControllerBase
    {
        private readonly FreeMedicalCampService _service;

        public FreeMedicalCampController(FreeMedicalCampService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCamp([FromBody] FreeMedicalCampCreateDto dto)
        {
            var camp = new FreeMedicalCamp
            {
                Title = dto.Title,
                Date = dto.Date,
                TimePeriod = dto.Time,
                Location = dto.Location,
                DoctorId = dto.DoctorId,
                Description = dto.Description
            };

            var created = await _service.CreateCampAsync(camp);
            return CreatedAtAction(nameof(GetCampById), new { id = created.Id }, created);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var camps = await _service.GetAllCampsAsync();
            return Ok(camps);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCampById(int id)
        {
            var camp = await _service.GetCampByIdAsync(id);
            return camp == null ? NotFound() : Ok(camp);
        }

        [HttpGet("doctor/{doctorId}")]
        public async Task<IActionResult> GetByDoctor(int doctorId)
        {
            var camps = await _service.GetCampsByDoctorAsync(doctorId);
            return Ok(camps);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteCampAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}
