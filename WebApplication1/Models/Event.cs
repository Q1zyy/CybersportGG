namespace WebApplication1.Models
{
	public class Event
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public byte[] Image { get; set; }

		public List<int> TeamsId { get; set; } = new List<int>();

		public List<int> MatchesId { get; set; } = new List<int>();

		public DateOnly StartDate { get; set; }

		public DateOnly EndDate { get; set;}

		

	}
}
