using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using NotifyAPI.Hubs;
using NotifyAPI.DTOs;
using NotifyAPI.Models;

[ApiController]
[Route("api/[controller]")]
public class DocumentsController : ControllerBase
{
    private readonly DocumentService _documentService;
    private readonly IHubContext<NotificationHub> _hubContext;

    public DocumentsController(DocumentService documentService, IHubContext<NotificationHub> hubContext)
    {
        _documentService = documentService;
        _hubContext = hubContext;
    }
 
    [HttpPost("upload")]
    [Authorize(Roles = "HR")]
    public async Task<IActionResult> Upload([FromForm] DocumentUploadRequestDTO request)
    {
        if (request.File == null)
            return BadRequest("No file provided");

        var uploadedBy = User.FindFirstValue(ClaimTypes.Name) ?? "Unknown";

        try
        {
            var document = await _documentService.UploadDocumentAsync(request.File, uploadedBy);

            // Notify all clients
            await _hubContext.Clients.All.SendAsync("NewDocumentUploaded", new
            {
                document.FileName,
                document.FilePath,
                document.UploadedAt,
                document.UploadedBy
            });

            return Ok(document);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch
        {
            return StatusCode(500, "An error occurred while uploading the document.");
        }
    }
}
