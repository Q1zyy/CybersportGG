using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Services
{
	public interface ITeamService
	{
		public Task CreateTeam(Team model);

		public Task<Team> GetTeam(int id);

		public Task AddPlayer(int id, int playerId);

		public Task DeletePlayer(int id, int playerId);

		public Task EditTeam(int id, Team model);

		public Task<IEnumerable<Team>> SearchTeams(string s);

	}
}
