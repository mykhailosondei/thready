using ApplicationCommon.DTOs.Post;
using ApplicationCommon.DTOs.User;
using ApplicationDAL.Entities;
using System.Text.Json.Serialization;

namespace ApplicationCommon.DTOs.Comment
{
    public class CommentDTO
    {
        [JsonIgnore]
        public int Id { get; set; }
        public int UserId { get; set; }

        public UserDTO Author { get; set; }

        [JsonIgnore]
        public int? PostId { get; set; }
        public PostDTO? Post { get; set; }
        
        [JsonIgnore]
        public int? CommentId { get; set; }
        public CommentDTO? ParentComment { get; set; }
        
        public DateTime CreatedAt { get; set; }
        public string TextContent { get; set; }
        public List<Image> Images { get; set; }
        public List<int> LikesIds { get; set; }
        public int Bookmarks { get; set; }
        public List<int> CommentsIds { get; set; }
        public List<int> ViewedBy { get; set; }

    }
}
