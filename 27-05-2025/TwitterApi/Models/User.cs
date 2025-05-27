using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TwitterAPI.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Username { get; set; } = null!;

        [Required, MaxLength(100)]
        public string Email { get; set; } = null!;

        [Required]
        public string PasswordHash { get; set; } = null!;

        public string? Bio { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Tweet> Tweets { get; set; } = new List<Tweet>();

        public ICollection<Like> Likes { get; set; } = new List<Like>();

        public ICollection<Follow> Followers { get; set; } = new List<Follow>();

        public ICollection<Follow> Following { get; set; } = new List<Follow>();
    }
}
