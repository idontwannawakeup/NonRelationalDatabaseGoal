namespace NonRelationalDatabaseGoal.Models;

public class User
{
    public string Id { get; set; } = default!;

    public string UserName { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string? Profession { get; set; }
    public string? Specialization { get; set; }
}
