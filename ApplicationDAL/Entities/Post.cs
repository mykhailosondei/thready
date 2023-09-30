
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
    public List<int> LikesIds { get; set; }
    public List<int> CommentsIds { get; set; }
    public List<int> RepostersIds { get; set; }
    public int Bookmarks { get; set; }
    public List<int> ViewedBy { get; set; }
}