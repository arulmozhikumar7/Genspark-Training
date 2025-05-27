using System.ComponentModel.DataAnnotations;

namespace TwitterAPI.Dtos
{
    public class TweetUpdateDto
    {
        [System.ComponentModel.DataAnnotations.Required]
        public int Id { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        public int UserId { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        [System.ComponentModel.DataAnnotations.MaxLength(280)]
        public string Content { get; set; } = null!;
    }
}
