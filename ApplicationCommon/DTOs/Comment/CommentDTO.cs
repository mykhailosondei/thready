using ApplicationCommon.DTOs.Like;
using ApplicationCommon.DTOs.Post;
using ApplicationCommon.DTOs.User;
using ApplicationCommon.Interfaces;
using ApplicationDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ApplicationCommon.DTOs.Comment
{
    public class CommentDTO
    {
        [JsonIgnore]
        public int Id { get; set; }
        [JsonIgnore]
        public int UserId { get; set; }
        [JsonIgnore]
        public int? PostId { get; set; }
        public PostDTO? Post { get; set; }
        
        [JsonIgnore]
        public int? CommentId { get; set; }
        public CommentDTO? ParentComment { get; set; }
        
        public DateTime Created { get; set; }
        public string TextContext { get; set; }
        public List<Image> Images { get; set; }
        public List<int> LikesIds { get; set; }
        public List<int> CommentsIds { get; set; }
        public List<int> ViewedBy { get; set; }

    }
}
