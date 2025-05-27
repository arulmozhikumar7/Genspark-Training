using System.Collections.Generic;
using System.Threading.Tasks;
using TwitterAPI.Interface;
using TwitterAPI.Models;

namespace TwitterAPI.Services
{
    public class TweetService
    {
        private readonly ITweetRepository _tweetRepository;

        public TweetService(ITweetRepository tweetRepository)
        {
            _tweetRepository = tweetRepository;
        }

        public async Task<Tweet?> GetByIdAsync(int id)
        {
            return await _tweetRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Tweet>> GetByUserIdAsync(int userId)
        {
            return await _tweetRepository.GetByUserIdAsync(userId);
        }

        public async Task<IEnumerable<Tweet>> GetAllAsync()
        {
            return await _tweetRepository.GetAllAsync();
        }

        public async Task AddAsync(Tweet tweet)
        {
            await _tweetRepository.AddAsync(tweet);
        }

        public async Task UpdateAsync(Tweet tweet)
        {
            await _tweetRepository.UpdateAsync(tweet);
        }

        public async Task DeleteAsync(int id)
        {
            await _tweetRepository.DeleteAsync(id);
        }
    }
}
