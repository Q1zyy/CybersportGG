using Humanizer.Bytes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Services;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
	public class PlayerController : Controller
	{

		private readonly IPlayerService _playerService;

		public PlayerController(IPlayerService playerService)
		{
			_playerService = playerService;
		}


		[HttpGet]
		[Authorize(Policy = "admin")]
		public IActionResult CreatePlayer()
		{
			return View();
		}
		
		
		[HttpPost]
		[Authorize(Policy = "admin")]
		public async Task<IActionResult> CreatePlayer(PlayerViewModel model)
		{
			byte[] bytes = null;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				await model.Image.CopyToAsync(memoryStream);
				bytes = memoryStream.ToArray();
			}

			await _playerService.CreatePlayer(new Models.Player {
				Nickname = model.Nickname,
				Name = model.Name,
				Age = model.Age,
				Image = bytes
			});

			return View();
		}

		[HttpGet("/player/{id:int}")]
		public async Task<IActionResult> Player(int id)
		{
			Player curPlayer = await _playerService.GetPlayer(id);
			ViewBag.CurrentPlayer = curPlayer;
			return View("Player");
		}

	}
}
