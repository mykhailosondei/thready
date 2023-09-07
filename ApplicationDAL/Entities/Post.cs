
using System.ComponentModel.DataAnnotations;

namespace ApplicationDAL.Entities;

public class Post : ICommentable
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    [Required]
    public User Author { get; set; }
    [Required]
    [MaxLength(500)]
    public string TextContent { get; set; }
    public List<Image> Images { get; set; }
    public List<Like> Likes { get; set; }
    public List<Comment> Comments { get; set; }
    public List<User> Reposts { get; set; }
    public int Bookmarks { get; set; }
    public List<User> ViewedBy { get; set; }
}