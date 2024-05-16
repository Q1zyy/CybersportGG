using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Services
{
	public interface ICommentService
	{
		public Task<IEnumerable<Comment>> GetComments(int id);

		public Task AddComment(Comment model);

		public Task DeleteComment(int id);

		public Task<Comment> GetComment(int id);

		public Task DeleteNewsComments(int id);


	}
}
