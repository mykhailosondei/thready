using ApplicationCommon.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ApplicationCommon.DTOs.Image;

namespace ApplicationCommon.DTOs.Comment
{
    public class CommentCreateDTO
    {
        [JsonIgnore]
        public int UserId { get; set; }
        public int? PostId { get; set; }
        public int? CommentId { get; set; }
        public string TextContent { get; set; }
        public List<ImageDTO> Images { get; set; }
    }
}
