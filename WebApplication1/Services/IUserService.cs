using System.Runtime.CompilerServices;
using WebApplication1.Models;

namespace WebApplication1.Services
{
	public interface IUserService
	{
		public Task<bool> HaveUser(string username);

		public Task<User> GetUser(string username);

		public Task AddUser(User user);

		public Task<IEnumerable<User>> GetUsers();	

		public Task ChangeRole(string username, string role);
	}
}
