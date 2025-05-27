using HospitalManagementAPI.Models;
using HospitalManagementAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HospitalManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentsController : ControllerBase
    {
        private readonly AppointmentService _appointmentService;

        public AppointmentsController(AppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        // POST: api/appointments
        [HttpPost]
        public async Task<IActionResult> AddAppointment([FromBody] Appointment appointment)
        {
            var result = await _appointmentService.AddAppointmentAsync(appointment);
            if (!result)
                return BadRequest("Appointment could not be scheduled due to conflicts or invalid data.");

            return Ok("Appointment added successfully.");
        }

        // PUT: api/appointments/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAppointment(int id, [FromBody] Appointment appointment)
        {
            if (id != appointment.Id)
                return BadRequest("Appointment ID mismatch.");

            await _appointmentService.UpdateAppointmentAsync(appointment);
            return Ok("Appointment updated successfully.");
        }

        // GET: api/appointments/doctor/{doctorId}
        [HttpGet("doctor/{doctorId}")]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetAppointmentsForDoctor(int doctorId)
        {
            var appointments = await _appointmentService.GetAppointmentsForDoctorAsync(doctorId);
            return Ok(appointments);
        }
    }
}
