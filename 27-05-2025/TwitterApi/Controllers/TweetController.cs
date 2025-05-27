using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwitterAPI.Dtos;
using TwitterAPI.Models;
using TwitterAPI.Services;


namespace TwitterAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TweetController : ControllerBase
    {
        private readonly TweetService _tweetService;

        public TweetController(TweetService tweetService)
        {
            _tweetService = tweetService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TweetReadDto>>> GetAll()
        {
            var tweets = await _tweetService.GetAllAsync();

            var tweetDtos = tweets.Select(t => new TweetReadDto
            {
                Id = t.Id,
                UserId = t.UserId,
                Content = t.Content,
                CreatedAt = t.CreatedAt
            });

            return Ok(tweetDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TweetReadDto>> GetById(int id)
        {
            var tweet = await _tweetService.GetByIdAsync(id);
            if (tweet == null)
                return NotFound();

            var tweetDto = new TweetReadDto
            {
                Id = tweet.Id,
                UserId = tweet.UserId,
                Content = tweet.Content,
                CreatedAt = tweet.CreatedAt
            };

            return Ok(tweetDto);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<TweetReadDto>>> GetByUserId(int userId)
        {
            var tweets = await _tweetService.GetByUserIdAsync(userId);

            var tweetDtos = tweets.Select(t => new TweetReadDto
            {
                Id = t.Id,
                UserId = t.UserId,
                Content = t.Content,
                CreatedAt = t.CreatedAt
            });

            return Ok(tweetDtos);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] TweetCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var tweet = new Tweet
            {
                UserId = dto.UserId,
                Content = dto.Content
            };

            await _tweetService.AddAsync(tweet);

            var tweetDto = new TweetReadDto
            {
                Id = tweet.Id,
                UserId = tweet.UserId,
                Content = tweet.Content,
                CreatedAt = tweet.CreatedAt
            };

            return CreatedAtAction(nameof(GetById), new { id = tweet.Id }, tweetDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] TweetUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != dto.Id)
                return BadRequest("ID mismatch");

            var existingTweet = await _tweetService.GetByIdAsync(id);
            if (existingTweet == null)
                return NotFound();

            existingTweet.Content = dto.Content;
            existingTweet.UserId = dto.UserId;

            await _tweetService.UpdateAsync(existingTweet);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existingTweet = await _tweetService.GetByIdAsync(id);
            if (existingTweet == null)
                return NotFound();

            await _tweetService.DeleteAsync(id);
            return NoContent();
        }
    }
}
