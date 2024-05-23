using WebApplication1.Models;

namespace WebApplication1.ViewModels
{
	public class ResultPageViewModel
	{

		public int Id { get; set; }

		public int Winner { get; set; }

		public string Score { get; set; }

		public Event Event { get; set; }

		public Team Team1 { get; set; }

		public Team Team2 { get; set; }

		public DateTime Date { get; set; }

	}
}
