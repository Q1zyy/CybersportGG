using WebApplication1.Models;

namespace WebApplication1.Services
{
	public interface IUserService
	{
		public bool HaveUser(string username);

		public User GetUser(string username);

		public void AddUser(User user);
	}
}
