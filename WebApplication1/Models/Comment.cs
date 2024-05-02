namespace WebApplication1.Models
{
	public class Comment
	{

		public int Id { get; set; }

		public int NewsId { get; set; }

		public string Content { get; set; }

		public string Author { get; set; }
	
	}
}
