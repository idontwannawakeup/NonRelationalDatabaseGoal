namespace NonRelationalDatabaseGoal.Parameters;

public class TicketParameters : QueryStringParameters
{
    public string? ProjectId { get; set; }
    public string? ExecutorId { get; set; }
    public string? Title { get; set; }
    public string? Status { get; set; }
}
