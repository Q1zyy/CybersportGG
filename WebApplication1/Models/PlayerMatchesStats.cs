namespace WebApplication1.Models
{
    public class PlayerMatchesStats
    {

        public int Id { get; set; }

        public int PlayerId { get; set; }

        public int MatchId { get; set; }

        public int Kills { get; set; }

        public int Deaths { get; set; }

        public int Assists { get; set; }

        public int Headshots { get; set; }

    }
}
