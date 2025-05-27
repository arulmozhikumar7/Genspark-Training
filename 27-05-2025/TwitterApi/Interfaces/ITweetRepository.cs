using System.Collections.Generic;
using System.Threading.Tasks;
using TwitterAPI.Models;

namespace TwitterAPI.Interface
{
    public interface ITweetRepository
    {
        Task<Tweet?> GetByIdAsync(int id);
        Task<IEnumerable<Tweet>> GetByUserIdAsync(int userId);
        Task<IEnumerable<Tweet>> GetAllAsync();

        Task AddAsync(Tweet tweet);
        Task UpdateAsync(Tweet tweet);
        Task DeleteAsync(int id);
    }
}
