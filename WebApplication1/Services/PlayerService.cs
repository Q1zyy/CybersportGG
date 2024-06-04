using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Services
{
	public class PlayerService : IPlayerService
	{
		
		private ApplicationDbContext db;

		public PlayerService(ApplicationDbContext context)
		{
			db = context;
		}

		public async Task ChangePlayer(Player model)
		{
			var player = await GetPlayer(model.Id);
			if (model.TeamId != -1)
			{
				player.TeamId = model.TeamId;
			}
			player.Name = model.Name;
			player.Nickname = model.Nickname;
			player.Age = model.Age;
			if (model.Image != null)
			{
				player.Image = model.Image;
			}
			await db.SaveChangesAsync();
		}

		public async Task CreatePlayer(Player model)
		{
			await db.Players.AddAsync(model);
			await db.SaveChangesAsync();
		}

		public async Task<Player> GetPlayer(int id)
		{
			return await db.Players.FirstOrDefaultAsync(p => p.Id == id);
		}

        public async Task<PlayerStats> GetPlayerStats(int id)
        {
            return await db.PlayerStats.FirstOrDefaultAsync(p => p.PlayerId == id);
        }

        public async Task<IEnumerable<Player>> Search(string s)
		{
			return await db.Players.Where(p => p.Nickname.Contains(s)).Take(5).ToListAsync();
		}
	}
}
