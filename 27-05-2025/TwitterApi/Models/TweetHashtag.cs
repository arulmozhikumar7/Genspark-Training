using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TwitterAPI.Models
{
    public class TweetHashtag
    {
        [Required]
        public int TweetId { get; set; }

        [Required]
        public int HashtagId { get; set; }

        [ForeignKey("TweetId")]
        public Tweet Tweet { get; set; } = null!;

        [ForeignKey("HashtagId")]
        public Hashtag Hashtag { get; set; } = null!;
    }
}
