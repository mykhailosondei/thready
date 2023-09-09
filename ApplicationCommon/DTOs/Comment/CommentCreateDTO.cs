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
    public class CommentCreateDTO
    {
        [JsonIgnore]
        public int AuthorId { get; set; }
        [JsonIgnore]
        public int? PostId { get; set; }
        [JsonIgnore]
        public int? CommentId { get; set; }
        public ICommentable Commentable { get; set; }
        public string TextContent { get; set; }
        public List<Image> Images { get; set; }
    }
}
