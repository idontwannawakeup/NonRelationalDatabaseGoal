using Newtonsoft.Json;

namespace NonRelationalDatabaseGoal.Models.Requests;

public class CreateUserRequest
{
    [JsonProperty(PropertyName = "userName")]
    public string UserName { get; set; } = default!;

    [JsonProperty(PropertyName = "firstName")]
    public string FirstName { get; set; } = default!;

    [JsonProperty(PropertyName = "lastName")]
    public string LastName { get; set; } = default!;

    [JsonProperty(PropertyName = "profession")]
    public string? Profession { get; set; }

    [JsonProperty(PropertyName = "specialization")]
    public string? Specialization { get; set; }
}
