namespace NonRelationalDatabaseGoal.Parameters;

public class UserParameters : QueryStringParameters
{
    public string? TeamId { get; set; }
    public string? UserName { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? FullName { get; set; }
}
