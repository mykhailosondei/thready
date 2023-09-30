using ApplicationCommon.DTOs.Post;
using ApplicationCommon.DTOs.User;
using ApplicationDAL.Entities;
using Newtonsoft.Json;

namespace ApplicationCommon.DTOs.Comment
{
    public class CommentDTO
    {
        [System.Text.Json.Serialization.JsonIgnore]
        public int Id { get; set; }
        public int UserId { get; set; }

        public UserDTO Author { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public int? PostId { get; set; }
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public PostDTO? Post { get; set; }
        
        [System.Text.Json.Serialization.JsonIgnore]
        public int? CommentId { get; set; }
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
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
