using Microsoft.Identity.Client;

namespace WebApplication1.Models
{
	public class News
	{
		public int Id { get; set; }

		public string Title { get; set; }

		public string Content { get; set; }

		public string Author { get; set; }

		public DateTime Date { get; set; }

		public bool IsShow { get; set; } = true;

	}
}
