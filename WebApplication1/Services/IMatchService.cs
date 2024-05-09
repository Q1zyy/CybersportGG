using WebApplication1.Models;

namespace WebApplication1.Services
{
	public interface IMatchService
	{

		public Task<Match> AddMatch(Match match);

		public Task RemoveMatch(Match match);

		public Task<IEnumerable<Match>> GetEventMatches(int id);

		public Task<Match> GetMatch(int id);

	}
}
