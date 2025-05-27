using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TwitterAPI.Models
{
    public class Follow
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int FollowerId { get; set; }

        [Required]
        public int FollowingId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("FollowerId")]
        public User Follower { get; set; } = null!;

        [ForeignKey("FollowingId")]
        public User Following { get; set; } = null!;
    }
}
