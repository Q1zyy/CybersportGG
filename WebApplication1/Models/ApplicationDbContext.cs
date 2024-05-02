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
