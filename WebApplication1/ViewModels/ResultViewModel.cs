using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApplication1.ViewModels
{
	public class ResultViewModel
	{

		public string Winner { get; set; }		

		public string Score { get; set; }

		public List<SelectListItem> Options { get; set; }

	}
}
