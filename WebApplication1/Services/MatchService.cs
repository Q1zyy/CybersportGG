using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Services
{
	public class MatchService : IMatchService
	{

		private ApplicationDbContext db;
		private readonly IResultService _resultService;

		public MatchService(ApplicationDbContext context, IResultService resultService)
		{
			db = context;
			_resultService = resultService;
		}

		public async Task<Match> AddMatch(Match match)
		{
			await db.Matches.AddAsync(match);
			await db.SaveChangesAsync();
			return match;
		}

		public async Task ChangeMatch(int id, Match match)
		{
			var m = await GetMatch(id);
			if (match.Team1 != null)
			{
				m.Team1 = match.Team1;
			}
			if (match.Team2 != null)
			{
				m.Team2 = match.Team2;
			}
			if (match.Date != null && match.Date != DateTime.MinValue)
			{
				m.Date = match.Date;
			}
			await db.SaveChangesAsync();
		}

		public async Task DeleteMatch(int id)
		{
			var match = await GetMatch(id);
			db.Matches.Remove(match);
			await db.SaveChangesAsync();
		}

		public async Task<IEnumerable<Match>> GetEventMatches(int id)
		{
			return await db.Matches.Where(m => m.EventId == id).ToListAsync();
		}
		
		public async Task<IEnumerable<Match>> GetUpcomingEventMatches(int id)
		{
			return await db.Matches.Where(m => m.EventId == id && !m.Done).ToListAsync();
		}

		public async Task<Match> GetMatch(int id)
		{
			return await db.Matches.FirstOrDefaultAsync(m => m.Id == id);
		}

        public async Task CompleteMatch(int id)
        {
			var m = await GetMatch(id);
			m.Done = true;
			await db.SaveChangesAsync();
        }
    }
}
