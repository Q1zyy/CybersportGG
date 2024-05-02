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
		public async Task<IActionResult> News()
		{
			var lst = await _newsService.GetNews();
			ViewBag.NewsList = lst;
			return View();
		}


		[HttpGet("/news/{id:int}")]
		public async Task<IActionResult> News(int id)
		{
			News curNews = await _newsService.GetSingleNews(id);
			ViewBag.CurrentNews = curNews;

			ViewBag.CurrentComments = await _commentService.GetComments(id);
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
		public async Task<IActionResult> CreateNews(NewsViewModel model)
		{

			await _newsService.CreateNews(new News()
			{
				Author = User.Identity.Name,
				Content = model.Content,
				Title = model.Title

			});

			return Redirect("/news");
		}


		[HttpGet]
		[Authorize(Policy = "writer")]
		public async Task<IActionResult> Delete(int id)
		{
			await _newsService.DeleteNews(id);
			return Redirect("/news");
		}
		
		[HttpGet]
		[Authorize(Policy = "writer")]
		public async Task<IActionResult> Edit(int id)
		{
			var news = await _newsService.GetSingleNews(id);
			ViewBag.NewsTitle = news.Title;
			ViewBag.NewsContent = news.Content;
			return View();
		}
		
		[HttpPost]
		[Authorize(Policy = "writer")]
		public async Task<IActionResult> Edit(int id, NewsViewModel model)
		{
			await _newsService.UpdateNews(id, new News()
			{
				Author = User.Identity.Name,
				Content = model.Content,
				Title = model.Title
			});
			return Redirect("/news");
		}

		[HttpPost]
		[Authorize]
		public async Task<IActionResult> AddComment(int id, CommentViewModel model)
		{
			await _commentService.AddComment(new Comment()
			{
				Content = model.Content,
				Author = User.Identity.Name,
				NewsId = id
			});
			return Redirect("/news/"+id);
		}

	}
}
