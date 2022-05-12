using NonRelationalDatabaseGoal.Models;
using NonRelationalDatabaseGoal.Parameters;

namespace NonRelationalDatabaseGoal.Extensions.Filtering;

public static class TicketFilteringExtensions
{
    public static IQueryable<Ticket> ApplyFiltering(
        this IQueryable<Ticket> tickets,
        TicketParameters parameters)
    {
        if (!string.IsNullOrWhiteSpace(parameters.ProjectId))
        {
            tickets = tickets.Where(ticket => ticket.ProjectId == parameters.ProjectId);
        }

        if (!string.IsNullOrWhiteSpace(parameters.ExecutorId))
        {
            tickets = tickets.Where(
                ticket => ticket.ExecutorId != null
                          && ticket.ExecutorId == parameters.ExecutorId);
        }

        if (!string.IsNullOrWhiteSpace(parameters.Title))
        {
            tickets = tickets.Where(ticket => ticket.Title.Contains(parameters.Title));
        }

        if (!string.IsNullOrWhiteSpace(parameters.Status))
        {
            tickets = tickets.Where(ticket => ticket.Status == parameters.Status);
        }

        return tickets;
    }
}
