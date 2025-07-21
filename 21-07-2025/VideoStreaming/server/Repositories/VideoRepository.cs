using Microsoft.EntityFrameworkCore;
using VideoApi.Contexts;
using VideoApi.Models;

namespace VideoApi.Repositories;

public class VideoRepository : IVideoRepository
{
    private readonly VideoDbContext _context;

    public VideoRepository(VideoDbContext context)
    {
        _context = context;
    }

    public async Task<List<TrainingVideo>> GetAllAsync() =>
        await _context.TrainingVideos.ToListAsync();

    public async Task<TrainingVideo> AddAsync(TrainingVideo video)
    {
        _context.TrainingVideos.Add(video);
        await _context.SaveChangesAsync();
        return video;
    }
    public async Task<TrainingVideo?> GetByIdAsync(int id)
    {
        return await _context.TrainingVideos.FindAsync(id);
    }
    public async Task<bool> ExistsByTitleAsync(string title)
    {
        return await _context.TrainingVideos.AnyAsync(v => v.Title == title);
    }
}
