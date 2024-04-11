using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Services
{
	public interface ICommentService
	{
		IEnumerable<Comment> GetComments(int id);

		void AddComment(Comment model);

	}
}
