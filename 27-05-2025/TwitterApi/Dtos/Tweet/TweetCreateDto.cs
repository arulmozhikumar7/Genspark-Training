using System.ComponentModel.DataAnnotations;

namespace TwitterAPI.Dtos
{
    public class TweetCreateDto
    {
        public int UserId { get; set; }

        [Required]
        [MaxLength(280)]
        public string Content { get; set; } = null!;
    }
}
