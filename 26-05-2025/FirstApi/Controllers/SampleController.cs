using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("/api/[controller]")]
public class SampleController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public ActionResult GetGreet()
    {
        bool success = true; 

        if (success)
        {
            return Ok("Sample API call successful.");
        }
        else
        {
            return BadRequest("Something went wrong.");
        }
    }
}
