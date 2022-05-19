using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using NonRelationalDatabaseGoal.Extensions;
using NonRelationalDatabaseGoal.Extensions.Filtering;
using NonRelationalDatabaseGoal.Models;
using NonRelationalDatabaseGoal.Parameters;

namespace NonRelationalDatabaseGoal.Services;

public class TicketService : GenericService<Ticket>
{
    public TicketService(CosmosClient client) : base(client.GetTicketsContainer())
    {
        UsersContainer = client.GetUsersContainer();
        ProjectsContainer = client.GetProjectsContainer();
    }

    protected Container UsersContainer { get; }
    protected Container ProjectsContainer { get; }

    public override async Task CreateAsync(Ticket ticket)
    {
        Project project = await ProjectsContainer.ReadItemAsync<Project>(
            ticket.ProjectId,
            new PartitionKey(ticket.ProjectId));

        project.Tickets.Add(ticket.Id);
        await ProjectsContainer.UpsertItemAsync(project, new PartitionKey(project.Id));

        if (!string.IsNullOrWhiteSpace(ticket.ExecutorId))
        {
            Models.User user = await UsersContainer.ReadItemAsync<Models.User>(
                ticket.ExecutorId,
                new PartitionKey(ticket.ExecutorId));

            user.AssignedTickets.Add(ticket.Id);
            await UsersContainer.UpsertItemAsync(user, new PartitionKey(user.Id));
        }

        await base.CreateAsync(ticket);
    }

    public override async Task DeleteAsync(string id)
    {
        Ticket ticket = await base.GetByIdAsync(id);
        Project project = await ProjectsContainer.ReadItemAsync<Project>(
            ticket.ProjectId,
            new PartitionKey(ticket.ProjectId));

        project.Tickets.Remove(ticket.Id);
        await ProjectsContainer.UpsertItemAsync(project, new PartitionKey(project.Id));

        if (!string.IsNullOrWhiteSpace(ticket.ExecutorId))
        {
            Models.User user = await UsersContainer.ReadItemAsync<Models.User>(
                ticket.ExecutorId,
                new PartitionKey(ticket.ExecutorId));

            user.AssignedTickets.Remove(ticket.Id);
            await UsersContainer.UpsertItemAsync(user, new PartitionKey(user.Id));
        }

        await base.DeleteAsync(ticket.Id);
    }

    public async Task<IEnumerable<Ticket>> GetAsync(TicketParameters parameters) =>
        await Container.GetItemLinqQueryable<Ticket>()
            .ApplyFiltering(parameters)
            .ApplyOrdering(parameters)
            .ApplyPagination(parameters)
            .ToFeedIterator()
            .ReadAllAsync();

    public async Task AssignExecutorAsync(string ticketId, string executorId)
    {
        Ticket ticket = await base.GetByIdAsync(ticketId);
        Models.User executor = await UsersContainer.ReadItemAsync<Models.User>(
            executorId,
            new PartitionKey(executorId));

        await TryRemoveAssignedExecutorAsync(ticket);
        ticket.ExecutorId = executor.Id;
        executor.AssignedTickets.Add(ticketId);
        await base.UpdateAsync(ticket);
    }

    public async Task RemoveExecutorAsync(string ticketId)
    {
        Ticket ticket = await base.GetByIdAsync(ticketId);
        await TryRemoveAssignedExecutorAsync(ticket);
    }

    private async Task TryRemoveAssignedExecutorAsync(Ticket ticket)
    {
        if (!string.IsNullOrWhiteSpace(ticket.ExecutorId))
        {
            Models.User executor = await UsersContainer.ReadItemAsync<Models.User>(
                ticket.ExecutorId,
                new PartitionKey(ticket.ExecutorId));

            ticket.ExecutorId = null;
            executor.AssignedTickets.Remove(ticket.Id);
            await base.UpdateAsync(ticket);
            await UsersContainer.UpsertItemAsync(
                executor,
                new PartitionKey(executor.Id));
        }
    }
}
