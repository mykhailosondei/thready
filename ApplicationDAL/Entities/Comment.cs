using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace ApplicationDAL.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        
        [ForeignKey("User")]
        public int UserId { get; set; }
        public User Author { get; set; }
        
        
        [ForeignKey("Post")]
        public int? PostId { get; set; }
        public Post? Post { get; set; }
        
        [ForeignKey("Comment")]
        public int? CommentId { get; set; }
        public Comment? ParentComment { get; set; }
        public List<Image> Images { get; set; }
        public List<Comment> Comments { get; set; }
        public List<Like> Likes { get; set; }
        public List<User> Reposts { get; set; }
        public int Bookmarks { get; set; }
        public List<int> ViewedBy { get; set; }

    }
}
