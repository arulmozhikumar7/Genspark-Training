using VideoApi.Models;

namespace VideoApi.Repositories;

public interface IVideoRepository
{
    Task<List<TrainingVideo>> GetAllAsync();
    Task<TrainingVideo> AddAsync(TrainingVideo video);
    Task<TrainingVideo?> GetByIdAsync(int id);
    Task<bool> ExistsByTitleAsync(string title);

}
