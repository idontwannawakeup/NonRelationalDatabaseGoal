using Microsoft.Azure.Cosmos;
using NonRelationalDatabaseGoal.Extensions;
using NonRelationalDatabaseGoal.Models;

namespace NonRelationalDatabaseGoal.Services;

public class TicketService : GenericService<Ticket>
{
    public TicketService(CosmosClient client) : base(client.GetTicketsContainer())
    {
    }
}
