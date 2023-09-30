using Newtonsoft.Json;

namespace ApplicationCommon.DTOs.User;

public class UserShortAccountDTO
{
    [JsonIgnore]
    public int Id { get; set; }
    public string Username { get; set; }
    public string Bio { get; set; }
    public List<int> FollowersIds { get; set; }
    public List<int> FollowingIds { get; set; }
}