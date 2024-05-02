using WebApplication1.Models;

namespace WebApplication1.Services
{
	public interface ITeamService
	{
		public Task CreateTeam(Team model);

		public Task<Team> GetTeam(int id);

		public Task AddPlayer(int id, int playerId);


	}
}
