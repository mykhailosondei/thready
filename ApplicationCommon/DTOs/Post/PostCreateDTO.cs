using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ApplicationCommon.DTOs.Image;

namespace ApplicationCommon.DTOs.Post
{
    public class PostCreateDTO
    {
        public int AuthorId { get; set; }
        public string TextContent { get; set; }
        public List<ImageDTO> Images { get; set; }
    }
}
