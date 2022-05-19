using Microsoft.Azure.Cosmos;
using NonRelationalDatabaseGoal.Extensions;
using NonRelationalDatabaseGoal.Models;

namespace NonRelationalDatabaseGoal.Seeding;

public class Seeder
{
    private readonly CosmosClient _client;
    private readonly ILogger<Seeder> _logger;

    private readonly string _userId = Guid.NewGuid().ToString();
    private readonly string _teamId = Guid.NewGuid().ToString();
    private readonly string _projectId = Guid.NewGuid().ToString();
    private readonly string _ticketId = Guid.NewGuid().ToString();

    public Seeder(CosmosClient client, ILogger<Seeder> logger) =>
        (_client, _logger) = (client, logger);

    public async Task SeedDatabaseAsync()
    {
        var user = new Models.User
        {
            Id = _userId,
            UserName = "User1",
            FirstName = "Ostap",
            LastName = "Nice",
            Profession = "Developer",
            Specialization = "Backend",
            Teams = new List<string> { _teamId },
            AssignedTickets = new List<string> { _ticketId }
        };

        var team = new Team
        {
            Id = _teamId,
            Name = "Amigos",
            LeaderId = _userId,
            Specialization = "Web Development",
            About = "Young and ambitious",
            Members = new List<string> { _userId },
            Projects = new List<string> { _projectId }
        };

        var project = new Project
        {
            Id = _userId,
            TeamId = _teamId,
            Title = "Blog",
            Type = "Website",
            Description = "Just a simple blog from small team",
            Tickets = new List<string> { _ticketId }
        };

        var ticket = new Ticket
        {
            Id = _ticketId,
            ProjectId = _projectId,
            ExecutorId = _userId,
            Title = "Fix bug",
            Description = "There's unknown bug. Just fix it.",
            Type = "Epic",
            Status = "Backlog",
            Priority = "Medium",
            CreatedAt = DateTime.Parse("1/6/2022 1:57:03 PM")
        };

        _logger.LogInformation("Starting database seeding...");
        await _client.GetUsersContainer().CreateItemAsync(user, new PartitionKey(user.Id));
        await _client.GetTeamsContainer().CreateItemAsync(team, new PartitionKey(team.Id));
        await _client.GetProjectsContainer().CreateItemAsync(project, new PartitionKey(project.Id));
        await _client.GetTicketsContainer().CreateItemAsync(ticket, new PartitionKey(ticket.Id));
        _logger.LogInformation("Database seeding finished.");
    }
}
