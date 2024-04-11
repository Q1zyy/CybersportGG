using System.Reflection;
using WebApplication1.Models;

namespace WebApplication1.Services
{
	public class UserService : IUserService
	{
		private string path = "D:\\Forum\\WebApplication1\\WebApplication1\\users.txt";

		public UserService()
		{

		}

		public bool HaveUser(string username)
		{
			using (StreamReader reader = new StreamReader(path))
			{
				string text = "";
				while (text != null)
				{
					text = reader.ReadLine();
					if (text == null) break;
					var a = text.Split(' ');
					if (a[0] == username)
					{
						return true;
					}
				}
			}
			return false;
		}

		
		public void AddUser(User user)
		{ 

			if (!HaveUser(user.Username))
			{
				using (StreamWriter writer = new StreamWriter(path, true))
				{
					writer.WriteLine(user.Username + " " + user.Password + " user");
				}

			}
		}

		public User GetUser(string username)
		{
			User result = null;
			using (StreamReader reader = new StreamReader(path))
			{
				string text = "";
				while (text != null)
				{
					text = reader.ReadLine();
					if (text == null) break;
					var a = text.Split(' ');
					if (a[0] == username)
					{
						result = new User();
						result.Username = a[0];
						result.Password = a[1];
						result.Role = a[2];
					}
				}
			}
			return result;
		}


		public IEnumerable<User> GetUsers()
		{
			List<User> users = new List<User>();
            using (StreamReader reader = new StreamReader(path))
            {
                string text = "";
                while (text != null)
                {
                    text = reader.ReadLine();
                    if (text == null) break;
                    var a = text.Split(' ');
					User result = new User();
                    result.Username = a[0];
                    result.Password = a[1];
                    result.Role = a[2];
					users.Add(result);
				}
            }
			return users;
        }

		public void ChangeRole(string username, string role)
		{
			IEnumerable<User> users = GetUsers();
			foreach (var user in users)
			{
				if (username.Equals(user.Username))
				{
					user.Role = role;
				}
			}

			using (StreamWriter writer = new StreamWriter(path))
			{
				foreach (var user in users)
				{
					writer.WriteLine(user.Username + " " + user.Password + " " + user.Role);
				}
			}

		}
	}


}
