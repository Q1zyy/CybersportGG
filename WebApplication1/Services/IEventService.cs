using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Services
{
	public interface IEventService
	{

		public Task AddEvent(Event model);

		public Task DeleteEvent(int id);

		public Task AddTeam(int id, int teamId);
			
		public Task<Event> GetEvent(int id);

		public Task<IEnumerable<Event>> GetOngoingEvents();

		public Task<IEnumerable<Event>> GetUpcomingEvents();

		public Task<IEnumerable<Event>> GetDoneEvents();

		public Task AddMatchToEvent(int id, int matchId);
		
		public Task DeleteTeam(int id, int teamId);

		public Task ChangeEvent(int id, Event model);

		public Task DeleteMatchFromEvent(int id, int matchId);


	}
}
