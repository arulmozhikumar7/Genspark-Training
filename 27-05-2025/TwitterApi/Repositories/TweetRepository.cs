using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TwitterAPI.Interface;
using TwitterAPI.Models;
using TwitterAPI.Data;

namespace TwitterAPI.Repositories
{
    public class TweetRepository : ITweetRepository
    {
        private readonly TwitterDbContext _context;

        public TweetRepository(TwitterDbContext context)
        {
            _context = context;
        }

        public async Task<Tweet?> GetByIdAsync(int id)
        {
            return await _context.Tweets
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<Tweet>> GetByUserIdAsync(int userId)
        {
            return await _context.Tweets
                .Include(t => t.User)
                .Where(t => t.UserId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Tweet>> GetAllAsync()
        {
            return await _context.Tweets
                .Include(t => t.User)
                .ToListAsync();
        }

        public async Task AddAsync(Tweet tweet)
        {
            await _context.Tweets.AddAsync(tweet);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Tweet tweet)
        {
            _context.Tweets.Update(tweet);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var tweet = await _context.Tweets.FindAsync(id);
            if (tweet != null)
            {
                _context.Tweets.Remove(tweet);
                await _context.SaveChangesAsync();
            }
        }
    }
}
