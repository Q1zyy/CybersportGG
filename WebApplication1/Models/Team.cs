namespace WebApplication1.Models
{
	public class Team
	{
		public int Id { get; set; }
		
		public string Name { get; set; }

		public byte[] Image { get; set; }

		public List<int> PlayersID { get; set; } = new List<int>();

	}
}
