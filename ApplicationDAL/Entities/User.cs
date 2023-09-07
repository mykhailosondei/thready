using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationDAL.Entities
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Email { get; set; }

        [Required]
        public DateOnly DateOfBirth { get; set; }

        [Required]
        [MaxLength(50)]
        public string Password { get; set; }

        [Required]
        [MaxLength(15)]
        public string Username { get; set; }

        [ForeignKey("Image")]
        public int ImageId { get; set; }
        public Image Avatar { get; set; }

        public List<Post> Posts { get; set; }

        public List<User> Followers { get; set; }
        public List<User> Following { get; set; }
        [MaxLength(100)]
        public string Bio { get; set; }
        [MaxLength(30)]
        public string Location { get; set; }
        public List<Post> BookmarkedPosts { get; set; }
    }
}
