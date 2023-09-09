using ApplicationDAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationCommon.DTOs.User;
using ApplicationCommon.DTOs.Comment;
using ApplicationCommon.Interfaces;
using System.Text.Json.Serialization;
using ApplicationCommon.DTOs.Like;

namespace ApplicationCommon.DTOs.Post
{
    public class PostDTO : ICommentable, ILikeable
    {
        [JsonIgnore]
        public int PostId { get; set; }
        public DateTime CreatedAt { get; set; }
        [JsonIgnore]
        public int UserId { get; set; }
        public UserDTO Author { get; set; }
        public string TextContent { get; set; }
        public List<Image> Images { get; set; }
        public List<LikeDTO> Likes { get; set; }
        public List<CommentDTO> Comments { get; set; }
        public List<UserDTO> Reposters { get; set; }
        public int Bookmarks { get; set; }
        public List<int> ViewedBy { get; set; }
        
    }
}
