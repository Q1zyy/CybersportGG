using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using WebApplication1.ViewModels;

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

		public async Task<IEnumerable<Match>> GetUpcomingMatches()
		{
			return await db.Matches.Where(m => !m.Done).ToListAsync();
		}

		public async Task<IEnumerable<Match>> GetDoneTeamMatches(int teamId)
		{
			return await db.Matches.Where(m => m.Done && (m.Team1 == teamId || m.Team2 == teamId)).ToListAsync();
		}

        public async Task WriteStats(int id, List<PlayerStatsViewModel> playerStats)
        {
            foreach (var item in playerStats)
			{
				var playerStat = db.PlayerMatchesStats.Add(new PlayerMatchesStats
				{
					PlayerId = item.Player.Id,
					MatchId = id,
					Kills = item.Kills,
					Deaths = item.Deaths,
					Assists = item.Assists,
					Headshots = item.Headshots
				});
				var player = await db.PlayerStats.FirstOrDefaultAsync(p => p.PlayerId == item.Player.Id);
				if (player == null)
				{
					db.PlayerStats.Add(new PlayerStats
					{
						PlayerId = item.Player.Id,
						Kills= item.Kills,
						Deaths= item.Deaths,
						Assists = item.Assists,
						Headshots = item.Headshots
					});
				} else
				{
					player.Kills += item.Kills;
					player.Deaths += item.Deaths;
					player.Assists += item.Assists;
					player.Headshots += item.Headshots;
				}

			}
			await db.SaveChangesAsync();
        }
    }
}
