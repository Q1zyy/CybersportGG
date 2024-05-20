using WebApplication1.Models;

namespace WebApplication1.ViewModels
{
	public class TeamsListViewModel
	{

		public int Id { get; set; }

		public List<Team> teams = new List<Team>();

	}
}
