using Newtonsoft.Json;

namespace NonRelationalDatabaseGoal.Models.Requests;

public class CreateTicketRequest
{
    [JsonProperty(PropertyName = "projectId")]
    public string ProjectId { get; set; } = default!;

    [JsonProperty(PropertyName = "executorId")]
    public string? ExecutorId { get; set; }

    [JsonProperty(PropertyName = "title")]
    public string Title { get; set; } = default!;

    [JsonProperty(PropertyName = "type")]
    public string? Type { get; set; }

    [JsonProperty(PropertyName = "description")]
    public string Description { get; set; } = default!;

    [JsonProperty(PropertyName = "status")]
    public string Status { get; set; } = default!;

    [JsonProperty(PropertyName = "priority")]
    public string Priority { get; set; } = default!;

    [JsonProperty(PropertyName = "deadline")]
    public DateTime? Deadline { get; set; }
}
