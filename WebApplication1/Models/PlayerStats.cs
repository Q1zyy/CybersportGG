namespace WebApplication1.Models
{
    public class PlayerStats
    {

        public int Id { get; set; }

        public int PlayerId { get; set; } = 0;

        public int Kills { get; set; } = 0;

        public int Deaths { get; set; } = 0;

        public int Assists { get; set; } = 0;

        public int Headshots { get; set; } = 0;

    }
}
