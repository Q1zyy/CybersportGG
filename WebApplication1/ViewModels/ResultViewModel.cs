using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication1.Models;

namespace WebApplication1.ViewModels
{
	public class ResultViewModel
	{

		public string Winner { get; set; }		

		public string Score { get; set; }

		public List<PlayerStatsViewModel> Players1 { get; set; }

		public List<PlayerStatsViewModel> Players2 { get; set; }
		
		public List<SelectListItem> Options { get; set; }

	}
}
