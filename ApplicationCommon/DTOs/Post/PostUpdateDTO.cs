using ApplicationDAL.Entities;
using Newtonsoft.Json;

namespace ApplicationCommon.DTOs.Post;

public class PostUpdateDTO
{
    [JsonIgnore]
    public int Id;
    public string TextContent { get; set; }
    public List<Image> Images { get; set; }
    
}