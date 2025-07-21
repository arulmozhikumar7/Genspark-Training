using Microsoft.EntityFrameworkCore;
using VideoApi.Models;

namespace VideoApi.Contexts;

public class VideoDbContext : DbContext
{
    public VideoDbContext(DbContextOptions<VideoDbContext> options) : base(options) { }

    public DbSet<TrainingVideo> TrainingVideos { get; set; }
}
