using Newtonsoft.Json;
using NonRelationalDatabaseGoal.Interfaces;

namespace NonRelationalDatabaseGoal.Models;

public class Project : IIdentifiable
{
    [JsonProperty(PropertyName = "id")]
    public string Id { get; set; } = string.Empty;

    [JsonProperty(PropertyName = "teamId")]
    public string TeamId { get; set; } = default!;

    [JsonProperty(PropertyName = "title")]
    public string Title { get; set; } = default!;

    [JsonProperty(PropertyName = "type")]
    public string? Type { get; set; }

    [JsonProperty(PropertyName = "url")]
    public string? Url { get; set; }

    [JsonProperty(PropertyName = "description")]
    public string? Description { get; set; }

    [JsonProperty(PropertyName = "tickets")]
    public ICollection<string> Tickets { get; set; } = new List<string>();
}
