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

		public async Task CreatePlayer(Player model)
		{
			await db.Players.AddAsync(model);
			await db.SaveChangesAsync();
		}

		public async Task<Player> GetPlayer(int id)
		{
			return await db.Players.FirstOrDefaultAsync(p => p.Id == id);
		}

		public async Task<IEnumerable<Player>> Search(string s)
		{
			return await db.Players.Where(p => p.Nickname.Contains(s)).ToListAsync();
		}
	}
}
