using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationDAL.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        
        [ForeignKey("User")]
        public int UserId { get; set; }
        [Required]
        public User Author { get; set; }
        
        
        [ForeignKey("Post")]
        public int? PostId { get; set; }
        public Post? Post { get; set; }
        
        [ForeignKey("Comment")]
        public int? CommentId { get; set; }
        public Comment? ParentComment { get; set; }
        
        public DateTime CreatedAt { get; set; }
        [Required]
        [MaxLength(200)]
        public string TextContent { get; set; }
        public List<Image> Images { get; set; }
        public List<int> CommentsIds { get; set; }
        public List<int> LikesIds { get; set; }
        public List<int> ViewedBy { get; set; }

    }
}
