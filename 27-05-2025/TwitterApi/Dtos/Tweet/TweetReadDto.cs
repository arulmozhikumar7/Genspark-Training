using System;
using System.Collections.Generic;

namespace TwitterAPI.Dtos
{
    public class TweetReadDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; } = null!;
        public DateTime CreatedAt { get; set; }

    }
}
