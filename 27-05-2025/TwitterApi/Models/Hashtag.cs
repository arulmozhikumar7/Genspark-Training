using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TwitterAPI.Models
{
    public class Hashtag
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Tag { get; set; } = null!;

        public ICollection<TweetHashtag> TweetHashtags { get; set; } = new List<TweetHashtag>();
    }
}
