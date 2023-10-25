
using ApplicationCommon.DTOs.User;
using System.Text.Json.Serialization;
using ApplicationCommon.DTOs.Image;

namespace ApplicationCommon.DTOs.Post
{
    public class PostDTO
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public int UserId { get; set; }
        public UserDTO Author { get; set; }
        public string TextContent { get; set; }
        public List<ImageDTO> Images { get; set; }
        public List<int> LikesIds { get; set; }
        public List<int> CommentsIds { get; set; }
        public List<int> RepostersIds { get; set; }
        public int Bookmarks { get; set; }
        public List<int> ViewedBy { get; set; }
        
    }
}
