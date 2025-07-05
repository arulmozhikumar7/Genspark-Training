using ExpenseTrackerAPI.Configurations; 
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ExpenseTrackerAPI.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    public class FeatureFlagsController : ControllerBase
    {
        private readonly FeatureFlags _flags;

        public FeatureFlagsController(IOptions<FeatureFlags> flags)
        {
            _flags = flags.Value;
        }

        [HttpGet]
        public IActionResult GetFlags()
        {
            return Ok(new
            {
                enableCsvExport = _flags.EnableCsvExport
            });
        }
    }
}
