using HospitalManagementAPI.Services;
using HospitalManagementAPI.Models;
using HospitalManagementAPI.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HospitalManagementAPI.Authorization.Handlers
{
    public class DoctorOwnsAppointmentRequirement : IAuthorizationRequirement {}

    public class DoctorOwnsAppointmentHandler : AuthorizationHandler<DoctorOwnsAppointmentRequirement>
    {
        private readonly HospitalDbContext _context;

        public DoctorOwnsAppointmentHandler(HospitalDbContext context)
        {
            _context = context;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, DoctorOwnsAppointmentRequirement requirement)
        {
            var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                return;

            // Get the user's doctorId 
            var user = await _context.Users
                                     .AsNoTracking()
                                     .FirstOrDefaultAsync(u => u.Id == userId && u.DoctorId.HasValue);

            if (user == null) return;

            int doctorId = user.DoctorId.Value;

            // Get appointmentId from route
            if (context.Resource is HttpContext httpContext &&
                httpContext.Request.RouteValues.TryGetValue("id", out var routeValue) &&
                int.TryParse(routeValue?.ToString(), out int appointmentId))
            {
                var appointment = await _context.Appointments
                                                .AsNoTracking()
                                                .FirstOrDefaultAsync(a => a.Id == appointmentId);

                if (appointment != null && appointment.DoctorId == doctorId)
                {
                    context.Succeed(requirement);
                }
            }
        }
    }
}
