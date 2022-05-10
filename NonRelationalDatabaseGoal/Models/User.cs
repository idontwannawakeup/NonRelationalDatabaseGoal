using Newtonsoft.Json;
using NonRelationalDatabaseGoal.Interfaces;

namespace NonRelationalDatabaseGoal.Models;

public class User : IIdentifiable
{
    [JsonProperty(PropertyName = "id")]
    public string Id { get; set; } = string.Empty;

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

    [JsonProperty(PropertyName = "teams")]
    public ICollection<string> Teams { get; set; } = Array.Empty<string>();
}
