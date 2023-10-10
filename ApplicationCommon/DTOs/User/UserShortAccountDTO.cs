using Newtonsoft.Json;

namespace ApplicationCommon.DTOs.User;

public class UserShortAccountDTO
{
    [JsonIgnore]
    public int Id { get; set; }
    public string Username { get; set; }
    public string Bio { get; set; }
    public int Followers { get; set; }
    public int Following { get; set; }
}