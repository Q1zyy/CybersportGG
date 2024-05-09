using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApplication1.ViewModels
{
	public class MatchViewModel
	{
		public DateTime Date { get; set; }

		public string Team1 { get; set; }
		
		public string Team2 { get; set; }

		public List<SelectListItem> Options { get; set; }


	}
}
