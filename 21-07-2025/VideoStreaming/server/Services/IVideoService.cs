using Microsoft.AspNetCore.Http;
using VideoApi.Models;

namespace VideoApi.Services;

public interface IVideoService
{
    Task<TrainingVideo> UploadAsync(IFormFile file, string title, string description);
    Task<List<TrainingVideo>> GetAllAsync();
    Task<TrainingVideo?> GetByIdAsync(int id);
    Task<bool> ExistsByTitleAsync(string title);

}
