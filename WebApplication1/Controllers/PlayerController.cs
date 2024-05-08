using Humanizer.Bytes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
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

			await _playerService.CreatePlayer(new Player {
				Nickname = model.Nickname,
				Name = model.Name,
				Age = model.Age,
				Image = bytes
			});

			return View();
		}

		[HttpGet]
		[Authorize(Policy = "admin")]
		public async Task<IActionResult> Edit(int id)
		{
			ViewBag.CurrentPlayer = await _playerService.GetPlayer(id);
			return View();
		}	
		
		[HttpPost]
		[Authorize(Policy = "admin")]
		public async Task<IActionResult> Edit(int id, PlayerViewModel model)
		{
			byte[] bytes = null;
			if (model.Image != null)
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					await model.Image.CopyToAsync(memoryStream);
					bytes = memoryStream.ToArray();
				}
			}

			await _playerService.ChangePlayer(new Player
			{
				Id = id,
				TeamId = -1,
				Nickname = model.Nickname,
				Name = model.Name,
				Age = model.Age,
				Image = bytes
			});
			return Redirect(id.ToString());
		}

		[HttpGet("/player/{id:int}")]
		public async Task<IActionResult> Player(int id)
		{
			Player curPlayer = await _playerService.GetPlayer(id);
			ViewBag.CurrentPlayer = curPlayer;
			ViewBag.Role = User.Claims.Where(x => x.Type == ClaimTypes.Role).Select(c => c.Value).SingleOrDefault();
			return View("Player");
		}

	}
}
