﻿namespace NonRelationalDatabaseGoal.Models;

public class Project
{
    public string Id { get; set; } = default!;
    public string TeamId { get; set; } = default!;

    public string Title { get; set; } = default!;
    public string? Type { get; set; }
    public string? Url { get; set; }
    public string? Description { get; set; }
}