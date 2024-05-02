namespace WebApplication1.Models
{
	public class Player
	{
		public int Id { get; set; }

		public byte[] Image { get; set; }

		public string Nickname { get; set; }

		public string Name { get; set; }

		public int Age { get; set; }
	
		public int TeamId { get; set; }
	}
}
