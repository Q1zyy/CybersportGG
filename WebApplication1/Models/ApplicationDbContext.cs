using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models
{
	public class ApplicationDbContext : DbContext
	{
		public DbSet<News> News { get; set; }
		
		public DbSet<User> Users { get; set; }
		
		public DbSet<Comment> Comments { get; set; }

		public DbSet<Player> Players { get; set; }

		public DbSet<Team> Teams { get; set; }

		public DbSet<Event> Events { get; set; }

		public DbSet<Match> Matches { get; set; }

		public DbSet<Result> Results { get; set; }

		public DbSet<PlayerStats> PlayerStats { get; set; }
		
		public DbSet<MatchStats> MatchesStats { get; set; }

		
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{
			Database.EnsureCreated();
		}
		public ApplicationDbContext()
		{
			Database.EnsureCreated();
		}

	}
}
