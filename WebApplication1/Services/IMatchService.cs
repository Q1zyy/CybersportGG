using WebApplication1.Models;

namespace WebApplication1.Services
{
	public interface IMatchService
	{

		public Task<Match> AddMatch(Match match);

		public Task DeleteMatch(int id);

		public Task ChangeMatch(int id, Match match);

		public Task<IEnumerable<Match>> GetEventMatches(int id);

		public Task<IEnumerable<Match>> GetUpcomingEventMatches(int id);

		public Task<Match> GetMatch(int id);

		public Task CompleteMatch(int id);

	}
}
