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

        public async Task DeleteComment(int id)
        {
			var com = await GetComment(id);
			db.Comments.Remove(com);
			await db.SaveChangesAsync();
        }

        public async Task DeleteNewsComments(int id)
        {
			var del = db.Comments.Where(com => com.NewsId == id);
			db.Comments.RemoveRange(del);
			await db.SaveChangesAsync();
        }

        public async Task<Comment> GetComment(int id)
        {
            return await db.Comments.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Comment>> GetComments(int id)
		{
			return await db.Comments.ToListAsync();
		}
	}
}
