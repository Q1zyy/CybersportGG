using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Services
{
	public class CommentService : ICommentService
	{

		private ApplicationDbContext db;

		public CommentService(ApplicationDbContext context)
		{
			db = context;
		}

		public async Task AddComment(Comment model)
		{
			await db.Comments.AddAsync(model);
			await db.SaveChangesAsync();
		}

		public async Task<IEnumerable<Comment>> GetComments(int id)
		{
			return await db.Comments.ToListAsync();
		}
	}
}
