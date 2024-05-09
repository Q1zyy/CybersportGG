using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Services
{
	public interface IEventService
	{

		public Task AddEvent(Event model);

		public Task AddTeam(int id, int teamId);
			
		public Task<Event> GetEvent(int id);

		public Task<IEnumerable<Event>> GetOngoingEvents();

		public Task AddMatchToEvent(int id, int matchId);
		
		public Task DeleteTeam(int id, int teamId);


	}
}
