namespace NonRelationalDatabaseGoal.Models;

public class Ticket
{
    public string Id { get; set; } = default!;
    public string ProjectId { get; set; } = default!;
    public string? ExecutorId { get; set; }

    public string Title { get; set; } = default!;
    public string? Type { get; set; }
    public string Description { get; set; } = default!;
    public string Status { get; set; } = default!;
    public string Priority { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public DateTime? Deadline { get; set; }
}
