namespace NonRelationalDatabaseGoal.Models;

public class Team
{
    public string Id { get; set; } = default!;
    public string? LeaderId { get; set; }

    public string Name { get; set; } = default!;
    public string? Specialization { get; set; } = default!;
    public string? About { get; set; } = default!;
}
