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
        public int UserId { get; set; }
        public int? PostId { get; set; }
        public int? CommentId { get; set; }
        public string TextContent { get; set; }
        public List<Image> Images { get; set; }
    }
}
