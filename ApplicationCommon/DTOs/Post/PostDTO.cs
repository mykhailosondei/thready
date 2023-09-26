using ApplicationDAL.Entities;
using ApplicationCommon.DTOs.User;
using System.Text.Json.Serialization;

namespace ApplicationCommon.DTOs.Post
{
    public class PostDTO
    {
        [JsonIgnore]
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public int UserId { get; set; }
        public UserDTO Author { get; set; }
        public string TextContent { get; set; }
        public List<Image> Images { get; set; }
        public List<int> LikesIds { get; set; }
        public List<int> CommentsIds { get; set; }
        public List<int> RepostersIds { get; set; }
        public int Bookmarks { get; set; }
        public List<int> ViewedBy { get; set; }
        
    }
}
