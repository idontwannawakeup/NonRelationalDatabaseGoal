using NonRelationalDatabaseGoal.Models;
using NonRelationalDatabaseGoal.Parameters;

namespace NonRelationalDatabaseGoal.Interfaces.Services;

public interface ITeamService
{
    Task<IEnumerable<Team>> GetAllAsync();
    Task<Team> GetByIdAsync(string id);
    Task CreateAsync(Team team);
    Task UpdateAsync(Team item);
    Task DeleteAsync(string id);
    Task<IEnumerable<Team>> GetAsync(TeamParameters parameters);
    Task<IEnumerable<AppUser>> GetMembersAsync(string teamId);
    Task AddMemberAsync(string teamId, string memberId);
    Task DeleteMemberAsync(string teamId, string memberId);
}
