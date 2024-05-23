using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Services
{
	public class EventService : IEventService
	{
		private ApplicationDbContext db;
		private readonly IMatchService _matchService;

		public EventService(ApplicationDbContext context, IMatchService matchService)
		{
			db = context;
			_matchService = matchService;
		}

		public async Task AddEvent(Event model)
		{
			await db.Events.AddAsync(model);
			await db.SaveChangesAsync();
		}

		public async Task AddTeam(int id, int teamId)
		{
			var e = await db.Events.FirstOrDefaultAsync(ev => ev.Id == id);
			e.TeamsId.Add(teamId);
			await db.SaveChangesAsync();
		}

		public async Task<Event> GetEvent(int id)
		{
			return await db.Events.FirstOrDefaultAsync(e => e.Id == id);
		}

		public async Task DeleteTeam(int id, int teamId)
		{
			var e = await db.Events.FirstOrDefaultAsync(ev => ev.Id == id);
			if (e != null)
			{
				e.TeamsId.Remove(teamId);
			}
			await db.SaveChangesAsync();
		}

		public async Task<IEnumerable<Event>> GetOngoingEvents()
		{
			var cur = DateOnly.FromDateTime(DateTime.Now);
			return await db.Events.Where(e => e.StartDate <= cur && e.EndDate >= cur).ToListAsync();
		}

		public async Task AddMatchToEvent(int id, int matchId)
		{
			var e = await GetEvent(id);
			e.MatchesId.Add(matchId);
			await db.SaveChangesAsync();
		}

		public async Task ChangeEvent(int id, Event model)
		{
			var e = await GetEvent(id);
			if (model.StartDate != null && model.StartDate != DateOnly.MinValue)
			{
				e.StartDate = model.StartDate;
			}
			if (model.EndDate != null && model.EndDate != DateOnly.MinValue)
			{
				e.EndDate = model.EndDate;
			}
			if (model.Image != null)
			{	
				e.Image = model.Image;
			}
			if (model.Name != null)
			{
				e.Name = model.Name;
			}
			await db.SaveChangesAsync();
		}

		public async Task DeleteMatchFromEvent(int id, int matchId)
		{
			var e = await GetEvent(id);
			e.MatchesId.Remove(matchId);
			await db.SaveChangesAsync();
		}

		public async Task DeleteEvent(int id)
		{
			var ev = await GetEvent(id);
			var lst = ev.MatchesId.ToList();
			db.Events.Remove(ev);
			foreach (var m in lst)
			{
				await _matchService.DeleteMatch(m);
			}
			await db.SaveChangesAsync();
		}

		public async Task<IEnumerable<Event>> GetUpcomingEvents()
		{
			var cur = DateOnly.FromDateTime(DateTime.Now);
			return await db.Events.Where(e => e.StartDate > cur).ToListAsync();
		}

		public async Task<IEnumerable<Event>> GetDoneEvents()
		{
			var cur = DateOnly.FromDateTime(DateTime.Now);
			return await db.Events.Where(e => e.EndDate < cur).ToListAsync();
		}
	}
}
