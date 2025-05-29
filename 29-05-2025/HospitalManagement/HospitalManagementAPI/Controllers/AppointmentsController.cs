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
    public class AppointmentsController : ControllerBase
    {
        private readonly AppointmentService _appointmentService;

        public AppointmentsController(AppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        // POST: api/appointments
       [HttpPost]
        public async Task<IActionResult> AddAppointment([FromBody] AppointmentAddDto dto)
        {
            var appointment = new Appointment
            {
                PatientId = dto.PatientId,
                DoctorId = dto.DoctorId,
                Date = dto.Date,
                TimePeriod = new TimePeriod(dto.StartTime, dto.EndTime),
                Reason = dto.Reason
            };

            var result = await _appointmentService.AddAppointmentAsync(appointment);
            if (!result)
                return BadRequest("Appointment could not be scheduled due to conflicts or invalid data.");

            return Ok("Appointment added successfully.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAppointment(int id, [FromBody] AppointmentUpdateDto dto)
        {
            
            var existingAppointment = await _appointmentService.GetAppointmentByIdAsync(id);
            if (existingAppointment == null)
                return NotFound("Appointment not found.");

           
            existingAppointment.IsCancelled = dto.IsCancelled;

            await _appointmentService.UpdateAppointmentAsync(existingAppointment);

            return Ok("Appointment updated successfully.");
        }


        [HttpGet("doctor/{doctorId}")]
        public async Task<ActionResult<IEnumerable<AppointmentReadDto>>> GetAppointmentsForDoctor(int doctorId)
        {
            var appointmentsDto = await _appointmentService.GetAppointmentsForDoctorAsync(doctorId);
            return Ok(appointmentsDto);
        }

    }
}
