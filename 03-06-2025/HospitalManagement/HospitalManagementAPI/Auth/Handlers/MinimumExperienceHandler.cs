using HospitalManagementAPI.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;

public class MinimumExperienceHandler : AuthorizationHandler<MinimumExperienceRequirement>
{
    private readonly DoctorService _doctorService;

    public MinimumExperienceHandler(DoctorService doctorService)
    {
        _doctorService = doctorService;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumExperienceRequirement requirement)
    {
        var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
        {
            return;
        }

        var doctor = await _doctorService.GetDoctorByUserIdAsync(userId);
        if (doctor == null) return;

        if (doctor.YearsOfExperience >= requirement.MinimumYears)
        {
            context.Succeed(requirement);
        }
    }
}
