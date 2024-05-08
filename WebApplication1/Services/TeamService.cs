using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Services
{
	public class TeamService : ITeamService
	{

		private ApplicationDbContext db;

		public TeamService(ApplicationDbContext context)
		{
			db = context;
		}

		public async Task AddPlayer(int id, int playerId)
		{
			var team = await GetTeam(id);
			team.PlayersID.Add(playerId);
			await db.SaveChangesAsync();
		}
		
		public async Task DeletePlayer(int id, int playerId)
		{
			var team = await GetTeam(id);
			team.PlayersID.Remove(playerId);
			await db.SaveChangesAsync();
		}

		public async Task CreateTeam(Team model)
		{
			await db.Teams.AddAsync(model);
			await db.SaveChangesAsync();
		}

		public async Task<Team> GetTeam(int id)
		{
			return await db.Teams.FirstOrDefaultAsync(t => t.Id == id);
		}

		public async Task EditTeam(int id, Team model)
		{
			var team = await GetTeam(id);
			team.Name = model.Name;
			if (model.Image != null)
			{
				team.Image = model.Image;
			}
			await db.SaveChangesAsync();
		}
	}
}
