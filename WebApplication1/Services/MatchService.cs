using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Services
{
	public class MatchService : IMatchService
	{

		private ApplicationDbContext db;

		public MatchService(ApplicationDbContext context)
		{
			db = context;
		}

		public async Task<Match> AddMatch(Match match)
		{
			await db.Matches.AddAsync(match);
			await db.SaveChangesAsync();
			return match;
		}

		public async Task<IEnumerable<Match>> GetEventMatches(int id)
		{
			return await db.Matches.Where(m => m.EventId == id).ToListAsync();
		}

		public async Task<Match> GetMatch(int id)
		{
			return await db.Matches.FirstOrDefaultAsync(m => m.Id == id);
		}

		public Task RemoveMatch(Match match)
		{
			throw new NotImplementedException();
		}
	}
}
