namespace WebApplication1.Models
{
	public class Match
	{

		public int Id { get; set; }

		public DateTime Date { get; set; }

		public int Team1 { get; set; }

		public int Team2 { get; set; }

		public int EventId { get; set; }

		public bool Done {  get; set; }

	}
}
