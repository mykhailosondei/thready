using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ApplicationCommon.Interfaces;

namespace ApplicationDAL.Entities
{
    public class User : IFollower, IFollowing
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Email { get; set; }

        [Required]
        public DateOnly DateOfBirth { get; set; }

        [Required]
        [MaxLength(200)]
        public string PasswordHash { get; set; }

        [Required]
        [MaxLength(15)]
        public string Username { get; set; }
        
        [ForeignKey("Image")]
        
        public int? ImageId { get; set; }
        public Image? Avatar { get; set; }

        public List<Post> Posts { get; set; }
        public int PostsCount { get; set; }

        public List<int> FollowersIds { get; set; }
        public List<int> FollowingIds { get; set; }
        [MaxLength(100)]
        public string? Bio { get; set; }
        [MaxLength(30)]
        public string? Location { get; set; }
        public List<int> BookmarkedPostsIds { get; set; }
        public List<int> RepostsIds { get; set; }
    }
}
