using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationCommon.DTOs.Image;
using ApplicationCommon.DTOs.Post;
using ApplicationCommon.Interfaces;

namespace ApplicationCommon.DTOs.User
{
    public class UserDTO : IFollower, IFollowing
    {
        public int Id { get; set; }
        public string Email { get; set; }

        public DateOnly DateOfBirth { get; set; }
        public string Password { get; set; }

        public string Username { get; set; }

        public int? ImageId { get; set; }
        public ImageDTO? Avatar { get; set; }

        public List<PostDTO> Posts { get; set; }
        public int PostCount { get; set; }

        public List<int> FollowersIds { get; set; }
        public List<int> FollowingIds { get; set; }
        public string? Bio { get; set; }
        public string? Location { get; set; }
        public List<int> BookmarkedPostsIds { get; set; }
        public List<int> RepostsIds { get; set; }
    }
}
