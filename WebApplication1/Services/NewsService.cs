using Microsoft.EntityFrameworkCore;
using System.Reflection;
using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Services
{
	public class NewsService : INewsService
	{
		private ApplicationDbContext db;
		public string path = @"D:\Forum\WebApplication1\WebApplication1\CollectionNews";
		public string newsLenPath = @"D:\Forum\WebApplication1\WebApplication1\NewsCount.txt";

		public NewsService(ApplicationDbContext context)
		{
			db = context;
		}

		public async Task CreateNews(News model)
		{
			db.News.Add(model);
			await db.SaveChangesAsync();
		}

		public async Task DeleteNews(int id)
		{
			var news = await GetSingleNews(id);
			if (news != null)
			{
				db.News.Remove(news);
				await db.SaveChangesAsync();
			}
		}

		public async Task<IEnumerable<News>> GetNews()
		{
			return await db.News.ToListAsync();
		}

		public async Task<News> GetSingleNews(int id)
		{
			
			return await db.News.FirstOrDefaultAsync(n => n.Id == id);
		}

		public async Task UpdateNews(int id, News newNews)
		{
			var news = await db.News.FirstOrDefaultAsync(n => n.Id == id);
			news.Content = newNews.Content;
			news.Title = newNews.Title;
			await db.SaveChangesAsync();
		}
	}
}
