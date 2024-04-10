using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Text;
using WebApplication1.ViewModels;
using static System.Net.Mime.MediaTypeNames;
using WebApplication1.Models;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication1.Controllers
{
	public class NewsController : Controller
	{
		[Route("/news")]
		[HttpGet]
		public IActionResult News()
		{
			string path = @"D:\Forum\WebApplication1\WebApplication1\CollectionNews";
			var newsLenPath = @"D:\Forum\WebApplication1\WebApplication1\NewsCount.txt";
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
				} catch
				{

				}
			}
			ViewBag.NewsList = lst;
			return View();
		}


		[HttpGet("/news/{id:int}")]
		public IActionResult News(int id)
		{
			string path = @"D:\Forum\WebApplication1\WebApplication1\CollectionNews";
			var len = new DirectoryInfo(path).GetFiles().Length;
			string pathfile = path + @"\news" + (id) + ".txt";
			using (StreamReader reader = new StreamReader(pathfile))
			{
				string title = reader.ReadLine();
				string author = reader.ReadLine();
				string content = reader.ReadToEnd();
				News cur = new News();
				cur.Title = title;
				cur.Content = content;
				cur.Author = author;
				cur.Id = id;
				ViewBag.CurrentNews = cur;
			}
			return View("SingleNews");
		}

		[HttpGet]
		[Authorize(Policy = "writer")]
		public IActionResult CreateNews()
		{
			return View();
		}

		[HttpPost]
		[Authorize(Policy = "writer")]
		public IActionResult CreateNews(NewsViewModel model)
		{
			string path = @"D:\Forum\WebApplication1\WebApplication1\CollectionNews";
			var newsLenPath = @"D:\Forum\WebApplication1\WebApplication1\NewsCount.txt";
			int len = 0;
			using (StreamReader reader = new StreamReader(newsLenPath))
			{
				len = int.Parse(reader.ReadLine());
			}
			string filepath = path + @"\news" + (len + 1) + ".txt";
			using (FileStream fs = System.IO.File.Create(filepath)) {}
			using (StreamWriter writer = new StreamWriter(filepath))
			{
				writer.WriteLine(model.Title);
				writer.WriteLine(User.Identity.Name);
				writer.Write(model.Content);
			}

			using (StreamWriter writer = new StreamWriter(newsLenPath))
			{
				writer.WriteLine((len + 1).ToString());
			}

			return Redirect("/news");
		}


		[HttpDelete]
		[Authorize(Policy = "writer")]
		public IActionResult Delete(int id)
		{
			string path = @"D:\Forum\WebApplication1\WebApplication1\CollectionNews";
			System.IO.File.Delete(path + @"\news" + id + ".txt");
			return Redirect("/news");
		}

		[HttpPatch]
		[Authorize(Policy = "writer")]
		public IActionResult Edit(int id)
		{
			string path = @"D:\Forum\WebApplication1\WebApplication1\CollectionNews";
			System.IO.File.Delete(path + @"\news" + id + ".txt");
			return Redirect("/news");
		}
	}
}
