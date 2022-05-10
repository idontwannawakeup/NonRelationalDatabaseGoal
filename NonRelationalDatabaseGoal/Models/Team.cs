using Newtonsoft.Json;
using NonRelationalDatabaseGoal.Interfaces;

namespace NonRelationalDatabaseGoal.Models;

public class Team : IIdentifiable
{
    [JsonProperty(PropertyName = "id")]
    public string Id { get; set; } = default!;

    [JsonProperty(PropertyName = "leaderId")]
    public string? LeaderId { get; set; }

    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; } = default!;

    [JsonProperty(PropertyName = "specialization")]
    public string? Specialization { get; set; } = default!;

    [JsonProperty(PropertyName = "about")]
    public string? About { get; set; } = default!;

    [JsonProperty(PropertyName = "members")]
    public ICollection<string> Members { get; set; } = Array.Empty<string>();
}
