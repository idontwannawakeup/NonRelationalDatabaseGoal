using Microsoft.Azure.Cosmos;
using NonRelationalDatabaseGoal.Extensions;
using NonRelationalDatabaseGoal.Models;

namespace NonRelationalDatabaseGoal.Services;

public class TicketService : GenericService<Ticket>
{
    protected Container UsersContainer { get; }

    public TicketService(CosmosClient client) : base(client.GetTicketsContainer())
    {
        UsersContainer = client.GetUsersContainer();
    }

    public async Task AssignExecutorAsync(string ticketId, string executorId)
    {
        Ticket ticket = await base.GetByIdAsync(ticketId);
        Models.User executor = await UsersContainer.ReadItemAsync<Models.User>(
            executorId,
            new PartitionKey(executorId));

        ticket.ExecutorId = executor.Id;
        await base.UpdateAsync(ticket);
    }
}
