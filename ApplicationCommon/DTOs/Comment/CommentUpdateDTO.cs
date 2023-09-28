using ApplicationDAL.Entities;
using Newtonsoft.Json;

namespace ApplicationCommon.DTOs.Comment;

public class CommentUpdateDTO
{
    [JsonIgnore]
    public int Id { get; set; }

    public string TextContent { get; set; }

    public List<Image> Images { get; set; }
}