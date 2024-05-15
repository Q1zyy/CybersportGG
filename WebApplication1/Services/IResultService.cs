using System.Runtime.InteropServices;
using WebApplication1.Models;
	
namespace WebApplication1.Services
{
	public interface IResultService
	{
		public Task AddResult(Result model);

		public Task<Result> GetResult(int id);



	}
}
