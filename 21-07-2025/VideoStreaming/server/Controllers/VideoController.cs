using Microsoft.AspNetCore.Mvc;
using Azure.Storage.Blobs;
using VideoApi.Services;
using Microsoft.Extensions.Configuration;

namespace VideoApi.Controllers;

[ApiController]
[Route("api/videos")]
public class VideoController : ControllerBase
{
    private readonly IVideoService _videoService;
    private readonly IConfiguration _configuration;

    public VideoController(IVideoService videoService, IConfiguration configuration)
    {
        _videoService = videoService;
        _configuration = configuration;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload([FromForm] IFormFile file, [FromForm] string title, [FromForm] string description)
    {
        if (file == null || file.Length == 0)
            return BadRequest("Video file is required.");

        var allowedMimeTypes = new[] {
            "video/mp4",
            "video/mpeg",
            "video/quicktime",
            "video/x-msvideo",
            "video/x-ms-wmv",
            "video/webm",
            "video/3gpp"
        };

        if (!allowedMimeTypes.Contains(file.ContentType))
            return BadRequest("Invalid file type. Please upload a valid video file.");
        try
        {
            var result = await _videoService.UploadAsync(file, title, description);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }


    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var videos = await _videoService.GetAllAsync();
        return Ok(videos);
    }

    [HttpGet("{id}/stream")]
    public async Task<IActionResult> Stream(int id)
    {
        var video = await _videoService.GetByIdAsync(id);
        if (video == null)
            return NotFound();

        var connectionString = _configuration.GetConnectionString("AzureBlobStorage");
        var containerClient = new BlobContainerClient(connectionString, "training-videos");

        var blobName = new Uri(video.BlobUrl).Segments.Last();
        var blobClient = containerClient.GetBlobClient(blobName);

        if (!await blobClient.ExistsAsync())
            return NotFound();

        var stream = await blobClient.OpenReadAsync();

        return File(stream, "video/mp4", enableRangeProcessing: true);
    }
}
