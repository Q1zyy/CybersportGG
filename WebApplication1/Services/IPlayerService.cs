using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Services
{
	public interface IPlayerService
	{
		public Task CreatePlayer(Player model);

		public Task<Player> GetPlayer(int id);

		public Task<IEnumerable<Player>> Search(string s);

	}
}
