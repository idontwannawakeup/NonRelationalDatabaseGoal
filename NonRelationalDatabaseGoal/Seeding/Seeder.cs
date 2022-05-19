using Microsoft.Azure.Cosmos;
using NonRelationalDatabaseGoal.Extensions;
using NonRelationalDatabaseGoal.Models;

namespace NonRelationalDatabaseGoal.Seeding;

public class Seeder
{
    private readonly CosmosClient _client;
    private readonly ILogger<Seeder> _logger;

    public Seeder(CosmosClient client, ILogger<Seeder> logger) =>
        (_client, _logger) = (client, logger);

    public async Task SeedDatabaseAsync()
    {
        var user = new Models.User
        {
            Id = Guid.NewGuid().ToString(),
            UserName = "User1",
            FirstName = "Ostap",
            LastName = "Nice",
            Profession = "Developer",
            Specialization = "Backend"
        };

        var team = new Team
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Amigos",
            LeaderId = user.Id,
            Specialization = "Web Development",
            About = "Young and ambitious"
        };

        var project = new Project
        {
            Id = user.Id,
            TeamId = team.Id,
            Title = "Blog",
            Type = "Website",
            Description = "Just a simple blog from small team"
        };

        var ticket = new Ticket
        {
            Id = Guid.NewGuid().ToString(),
            ProjectId = project.Id,
            ExecutorId = user.Id,
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
