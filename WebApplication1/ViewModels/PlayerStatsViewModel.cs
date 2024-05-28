using WebApplication1.Models;

namespace WebApplication1.ViewModels
{
    public class PlayerStatsViewModel
    {

        public Player Player { get; set; } 

        public int Kills { get; set; }

        public int Deaths { get; set; }

        public int Assists { get; set; }

        public int Headshots { get; set; }


    }
}
