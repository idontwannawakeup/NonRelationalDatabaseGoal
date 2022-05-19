using NonRelationalDatabaseGoal.Models;
using NonRelationalDatabaseGoal.Parameters;

namespace NonRelationalDatabaseGoal.Interfaces.Services;

public interface ITicketService
{
    Task<IEnumerable<Ticket>> GetAllAsync();
    Task<Ticket> GetByIdAsync(string id);
    Task CreateAsync(Ticket ticket);
    Task UpdateAsync(Ticket ticket);
    Task DeleteAsync(string id);
    Task<IEnumerable<Ticket>> GetAsync(TicketParameters parameters);
    Task AssignExecutorAsync(string ticketId, string executorId);
    Task RemoveExecutorAsync(string ticketId);
}
