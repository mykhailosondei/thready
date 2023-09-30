using Newtonsoft.Json;

namespace ApplicationCommon.DTOs.User;

public class UpdateUserDTO
{
    [JsonIgnore]
    public int Id { get; set; }
    public string Bio { get; set; }
    public string Location { get; set; }
    public DateOnly DateOfBirth { get; set; }
}