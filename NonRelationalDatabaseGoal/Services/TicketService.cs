using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using NonRelationalDatabaseGoal.Extensions;
using NonRelationalDatabaseGoal.Extensions.Filtering;
using NonRelationalDatabaseGoal.Models;
using NonRelationalDatabaseGoal.Parameters;

namespace NonRelationalDatabaseGoal.Services;

public class TicketService : GenericService<Ticket>
{
    protected Container UsersContainer { get; }

    public TicketService(CosmosClient client) : base(client.GetTicketsContainer())
    {
        UsersContainer = client.GetUsersContainer();
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

            executor.AssignedTickets.Remove(ticket.Id);
            await UsersContainer.UpsertItemAsync(
                executor,
                new PartitionKey(executor.Id));
        }
    }
}
