using Newtonsoft.Json;

namespace NonRelationalDatabaseGoal.Models.Requests;

public class CreateProjectRequest
{
    [JsonProperty(PropertyName = "teamId")]
    public string TeamId { get; set; } = default!;

    [JsonProperty(PropertyName = "title")]
    public string Title { get; set; } = default!;

    [JsonProperty(PropertyName = "type")]
    public string? Type { get; set; }

    [JsonProperty(PropertyName = "url")]
    public string? Url { get; set; }
}
