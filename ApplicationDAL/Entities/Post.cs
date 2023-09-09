
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ApplicationDAL.Entities;

public class Post
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    
    [ForeignKey("User")]
    public int UserId { get; set; }
    [Required]
    public User Author { get; set; }
    [Required]
    [MaxLength(500)]
    public string TextContent { get; set; }
    public List<Image> Images { get; set; }
    public List<Like> Likes { get; set; }
    public List<Comment> Comments { get; set; }
    public List<User> Reposters { get; set; }
    public int Bookmarks { get; set; }
    public List<int> ViewedBy { get; set; }
}