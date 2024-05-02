using Microsoft.EntityFrameworkCore;
using System.Reflection;
using WebApplication1.Models;

namespace WebApplication1.Services
{
	public class UserService : IUserService
	{
		private ApplicationDbContext db;

		public UserService(ApplicationDbContext context)
		{
			db = context;
		}

		public async Task<bool> HaveUser(string username)
		{
			var user = await db.Users.FirstOrDefaultAsync(u => u.Username == username);
			return user == null ? false : true;
		}

		
		public async Task AddUser(User user)
		{ 
			db.Users.Add(user);
			await db.SaveChangesAsync();
		}

		public async Task<User> GetUser(string username)
		{
			return await db.Users.FirstOrDefaultAsync(u => u.Username.Equals(username));
		}


		public async Task<IEnumerable<User>> GetUsers()
		{
			return await db.Users.ToListAsync();
		}

		public async Task ChangeRole(string username, string role)
		{
			var user = await GetUser(username);
			user.Role = role;
			db.SaveChanges();
		}
	}


}
