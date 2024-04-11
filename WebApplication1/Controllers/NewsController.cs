using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Text;
using WebApplication1.ViewModels;
using static System.Net.Mime.MediaTypeNames;
using WebApplication1.Models;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
	public class NewsController : Controller
	{
		private readonly ICommentService _commentService;
		private readonly INewsService _newsService;

		public NewsController(INewsService newsService, ICommentService commentService)
		{
			_commentService = commentService;
			_newsService = newsService;
		}

		[Route("/news")]
		[HttpGet]
		public IActionResult News()
		{
			var lst = _newsService.GetNews();
			ViewBag.NewsList = lst;
			return View();
		}


		[HttpGet("/news/{id:int}")]
		public IActionResult News(int id)
		{
			News curNews = _newsService.GetnSingleNews(id);
			ViewBag.CurrentNews = curNews;

			ViewBag.CurrentComments = _commentService.GetComments(id);
			ViewBag.CurrentId = id;

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

			_newsService.CreateNews(new News()
			{
				Author = User.Identity.Name,
				Content = model.Content,
				Title = model.Title

			});

			return Redirect("/news");
		}


		[HttpGet]
		[Authorize(Policy = "writer")]
		public IActionResult Delete(int id)
		{
			_newsService.DeleteNews(id);
			return Redirect("/news");
		}
		
		[HttpGet]
		[Authorize(Policy = "writer")]
		public IActionResult Edit(int id)
		{
			var news = _newsService.GetnSingleNews(id);
			ViewBag.NewsTitle = news.Title;
			ViewBag.NewsContent = news.Content;
			return View();
		}
		
		[HttpPost]
		[Authorize(Policy = "writer")]
		public IActionResult Edit(int id, NewsViewModel model)
		{
			_newsService.UpdateNews(id, new News()
			{
				Author = User.Identity.Name,
				Content = model.Content,
				Title = model.Title
			});
			return Redirect("/news");
		}

		[HttpPost]
		[Authorize]
		public IActionResult AddComment(int id, CommentViewModel model)
		{
			_commentService.AddComment(new Comment()
			{
				Content = model.Content,
				Author = User.Identity.Name,
				NewsId = id
			});
			return Redirect("/news/"+id);
		}

	}
}
