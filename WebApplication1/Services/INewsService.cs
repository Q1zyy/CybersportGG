using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Services
{
	public interface INewsService
	{
		public IEnumerable<News> GetNews();

		public News GetnSingleNews(int id);

		public void CreateNews(News newsViewModel);

		public void UpdateNews(News newNews);

		public void DeleteNews(int id);

	}
}
