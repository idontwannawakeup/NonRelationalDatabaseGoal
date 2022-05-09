using Newtonsoft.Json;
using NonRelationalDatabaseGoal.Interfaces;

namespace NonRelationalDatabaseGoal.Models;

public class Team : IIdentifiable
{
    [JsonProperty(PropertyName = "id")]
    public string Id { get; set; } = default!;
    public string? LeaderId { get; set; }

    public string Name { get; set; } = default!;
    public string? Specialization { get; set; } = default!;
    public string? About { get; set; } = default!;
}
