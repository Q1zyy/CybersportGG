using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Services
{
	public interface INewsService
	{
		public Task<IEnumerable<News>> GetNews();

		public Task<News> GetSingleNews(int id);

		public Task CreateNews(News newsViewModel);

		public Task UpdateNews(int id, News newNews);

		public Task DeleteNews(int id);

	}
}
