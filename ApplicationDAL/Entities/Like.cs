using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationDAL.Entities;

public class Like
{
    public int Id { get; set; }
    [ForeignKey("User")]
    public int UserId { get; set; }
    [Required]
    public User User { get; set; }

    [ForeignKey("Post")]
    public int? PostId { get; set; }
    public Post? Post { get; set; }
    [ForeignKey("Comment")]
    public int? CommentId { get; set; }
    public Comment? Comment { get; set; }
}