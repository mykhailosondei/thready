using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationDAL.Entities;
using ApplicationCommon.DTOs.Post;

namespace ApplicationCommon.DTOs.User
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Email { get; set; }

        public DateOnly? DateOfBirth { get; set; }
        public string Password { get; set; }

        public string Username { get; set; }

        public int? ImageId { get; set; }
        public Image? Avatar { get; set; }

        public List<PostDTO> Posts { get; set; }

        public List<UserDTO> Followers { get; set; }
        public List<UserDTO> Following { get; set; }
        public string? Bio { get; set; }
        public string? Location { get; set; }
        public List<PostDTO> BookmarkedPosts { get; set; }
        public List<PostDTO> Reposts { get; set; }
    }
}
