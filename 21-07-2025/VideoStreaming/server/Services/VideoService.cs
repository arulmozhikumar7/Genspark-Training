using Azure.Storage.Blobs;
using VideoApi.Models;
using VideoApi.Repositories;

namespace VideoApi.Services;

public class VideoService : IVideoService
{
    private readonly IVideoRepository _repository;
    private readonly IConfiguration _configuration;

    public VideoService(IVideoRepository repository, IConfiguration configuration)
    {
        _repository = repository;
        _configuration = configuration;
    }

    public async Task<TrainingVideo> UploadAsync(IFormFile file, string title, string description)
    {
        if (await ExistsByTitleAsync(title))
        throw new InvalidOperationException("A video with this title already exists.");

        var containerName = "training-videos";
        var connectionString = _configuration.GetConnectionString("AzureBlobStorage");

        var blobClient = new BlobContainerClient(connectionString, containerName);
        await blobClient.CreateIfNotExistsAsync();

        var blobName = Guid.NewGuid() + Path.GetExtension(file.FileName);
        var blob = blobClient.GetBlobClient(blobName);

        using (var stream = file.OpenReadStream())
        {
            await blob.UploadAsync(stream);
        }

        var video = new TrainingVideo
        {
            Title = title,
            Description = description,
            BlobUrl = blob.Uri.ToString(),
            UploadDate = DateTime.UtcNow
        };

        return await _repository.AddAsync(video);
    }

    public async Task<List<TrainingVideo>> GetAllAsync() => await _repository.GetAllAsync();

    public async Task<TrainingVideo?> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }
    
    public async Task<bool> ExistsByTitleAsync(string title)
    {
        return await _repository.ExistsByTitleAsync(title);
    }

}
