namespace NonRelationalDatabaseGoal.Parameters;

public class TeamParameters : QueryStringParameters
{
    public string? MemberId { get; set; }
    public string? Name { get; set; }
    public string? Specialization { get; set; }
}
