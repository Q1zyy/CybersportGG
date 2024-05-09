namespace WebApplication1.ViewModels
{
	public class EventViewModel
	{
		public string Name { get; set; }

		public IFormFile Image { get; set; }
 
		public DateOnly StartDate { get; set; }

		public DateOnly EndDate { get; set; }
	}
}
