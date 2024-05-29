namespace WebApplication1.Models
{
    public class MatchStats
    {
        
        public int Id { get; set; }

        public int MatchId { get; set; }

        public List<PlayerStatsForDb> Player1Stats { get; set; } = new List<PlayerStatsForDb>();
        
        public List<PlayerStatsForDb> Player2Stats { get; set; } = new List<PlayerStatsForDb>();

    }
}
