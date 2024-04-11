using System.Reflection;
using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Services
{
	public class NewsService : INewsService
	{

		public string path = @"D:\Forum\WebApplication1\WebApplication1\CollectionNews";
		public string newsLenPath = @"D:\Forum\WebApplication1\WebApplication1\NewsCount.txt";

		public NewsService()
		{

		}

		public void CreateNews(News model)
		{
			int len = 0;
			using (StreamReader reader = new StreamReader(newsLenPath))
			{
				len = int.Parse(reader.ReadLine());
			}
			string filepath = path + @"\news" + (len + 1) + ".txt";
			using (FileStream fs = System.IO.File.Create(filepath)) { }
			using (StreamWriter writer = new StreamWriter(filepath))
			{
				writer.WriteLine(model.Title);
				writer.WriteLine(model.Author);
				writer.Write(model.Content);
			}

			using (StreamWriter writer = new StreamWriter(newsLenPath))
			{
				writer.WriteLine((len + 1).ToString());
			}
		}

		public void DeleteNews(int id)
		{
			System.IO.File.Delete(path + @"\news" + id + ".txt");
		}

		public IEnumerable<News> GetNews()
		{
			int len = 0;
			using (StreamReader reader = new StreamReader(newsLenPath))
			{
				len = int.Parse(reader.ReadLine());
			}

			var lst = new List<News>();
			for (int i = 0; i < len; i++)
			{
				string pathfile = path + @"\news" + (i + 1) + ".txt";
				try
				{

					using (StreamReader reader = new StreamReader(pathfile))
					{
						string title = reader.ReadLine();
						string author = reader.ReadLine();
						string content = reader.ReadToEnd();
						News cur = new News();
						cur.Title = title;
						cur.Content = content;
						cur.Author = author;
						cur.Id = i + 1;
						lst.Add(cur);
					}
				}
				catch {}
			}
			return lst;
		}

		public News GetnSingleNews(int id)
		{
			string pathfile = path + @"\news" + (id) + ".txt";
			News cur = new News();
			using (StreamReader reader = new StreamReader(pathfile))
			{
				string title = reader.ReadLine();
				string author = reader.ReadLine();
				string content = reader.ReadToEnd();
				cur.Title = title;
				cur.Content = content;
				cur.Author = author;
				cur.Id = id;
			}
			return cur;
		}

		public void UpdateNews(int id, News newNews)
		{
			string filepath = path + @"\news" + (id) + ".txt";
			using (StreamWriter writer = new StreamWriter(filepath))
			{
				writer.WriteLine(newNews.Title);
				writer.WriteLine(newNews.Author);
				writer.Write(newNews.Content);
			}
		}
	}
}
