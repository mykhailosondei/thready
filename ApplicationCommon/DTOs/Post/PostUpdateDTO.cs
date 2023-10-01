using ApplicationCommon.DTOs.Image;
using Newtonsoft.Json;

namespace ApplicationCommon.DTOs.Post;

public class PostUpdateDTO
{
    [JsonIgnore]
    public int Id;
    public string TextContent { get; set; }
    public List<ImageDTO> Images { get; set; }
    
}