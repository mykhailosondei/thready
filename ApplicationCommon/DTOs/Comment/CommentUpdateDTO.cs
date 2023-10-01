using ApplicationCommon.DTOs.Image;

using Newtonsoft.Json;

namespace ApplicationCommon.DTOs.Comment;

public class CommentUpdateDTO
{
    [JsonIgnore]
    public int Id { get; set; }

    public string TextContent { get; set; }

    public List<ImageDTO> Images { get; set; }
}