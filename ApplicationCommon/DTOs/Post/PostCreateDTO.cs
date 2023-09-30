using ApplicationDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ApplicationCommon.DTOs.Post
{
    public class PostCreateDTO
    {
        public int AuthorId { get; set; }
        public string TextContent { get; set; }
        public List<Image> Images { get; set; }
    }
}
