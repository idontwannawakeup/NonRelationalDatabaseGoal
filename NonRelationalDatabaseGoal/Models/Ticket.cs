using Newtonsoft.Json;
using NonRelationalDatabaseGoal.Interfaces;

namespace NonRelationalDatabaseGoal.Models;

public class Ticket : IIdentifiable
{
    [JsonProperty(PropertyName = "id")]
    public string Id { get; set; } = string.Empty;

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

    [JsonProperty(PropertyName = "createdAt")]
    public DateTime CreatedAt { get; set; }

    [JsonProperty(PropertyName = "deadline")]
    public DateTime? Deadline { get; set; }
}
