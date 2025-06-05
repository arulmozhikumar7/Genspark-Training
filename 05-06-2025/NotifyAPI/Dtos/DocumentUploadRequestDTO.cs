using Microsoft.AspNetCore.Http;
namespace NotifyAPI.DTOs{
public class DocumentUploadRequestDTO
{
    public IFormFile File { get; set; }
}
}