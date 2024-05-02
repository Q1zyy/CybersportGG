using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using WebApplication1.Models;
using WebApplication1.Services;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
	public class TeamController : Controller
	{
		private readonly ITeamService _teamService;
		private readonly IPlayerService _playerService;
		
		public TeamController(ITeamService teamService, IPlayerService playerService) {
			_teamService = teamService;
			_playerService = playerService;
		}

		[HttpGet]
		[Authorize(Policy = "admin")]
		public IActionResult CreateTeam() 
		{
			return View(); 
		}

		[HttpPost]
		[Authorize(Policy = "admin")]
		public async Task<IActionResult> CreateTeam(TeamViewModel model)
		{
			byte[] bytes = null;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				await model.Image.CopyToAsync(memoryStream);
				bytes = memoryStream.ToArray();
			}

			await _teamService.CreateTeam(new Models.Team
			{
				Name = model.Name,
				Image = bytes
			});

			return View();
		}

		[HttpGet("/team/{id:int}")]
		public async Task<IActionResult> Team(int id)
		{
			ViewBag.Role = User.Claims.Where(x => x.Type == ClaimTypes.Role).Select(c => c.Value).SingleOrDefault();
			Team curTeam = await _teamService.GetTeam(id);
			ViewBag.CurrentTeam = curTeam;
			ViewBag.CurrentId = id;
			List<Player> list = new List<Player>();
			if (curTeam != null && curTeam.PlayersID != null)
			{
				foreach (var ID in curTeam.PlayersID)
				{
					list.Add(await _playerService.GetPlayer(ID));
				}
			}
			ViewBag.CurrentPlayers = list;
			return View("Team");
		}	
		

		[HttpPost]
		[Authorize(Policy = "admin")]
		public async Task<IActionResult> SearchPlayers(int id)
		{
			string str = Request.Form["searchString"];
			var players = await _playerService.Search(str);
			ViewBag.Role = User.Claims.Where(x => x.Type == ClaimTypes.Role).Select(c => c.Value).SingleOrDefault();
			ViewBag.Players = players;
			Team curTeam = await _teamService.GetTeam(id);
			ViewBag.CurrentTeam = curTeam;
			ViewBag.CurrentId = id;
			List<Player> list = new List<Player>();
			if (curTeam != null && curTeam.PlayersID != null)
			{
				foreach (var ID in curTeam.PlayersID)
				{
					list.Add(await _playerService.GetPlayer(ID));
				}
			}
			ViewBag.CurrentPlayers = list;
			return View("Team");
		}



		public async Task<IActionResult> AddPlayer(int id, int teamId)
		{
			var player = await _playerService.GetPlayer(id);
			var team = await _teamService.GetTeam(teamId);
			await _playerService.ChangePlayer(new Player {
				Id = id,
				Nickname = player.Nickname,
				Name = player.Name,
				Age = player.Age,
				TeamId = teamId
			});
			await _teamService.AddPlayer(teamId, id);
			return Redirect(teamId.ToString());
		}
		
		public async Task<IActionResult> DeletePlayer(int id, int teamId)
		{
			var player = await _playerService.GetPlayer(id);
			var team = await _teamService.GetTeam(teamId);
			await _teamService.DeletePlayer(teamId, id);
			await _playerService.ChangePlayer(new Player
			{
				TeamId = 0,
				Id = id,
				Nickname = player.Nickname,
				Name = player.Name,
				Age = player.Age
			});
			return Redirect(teamId.ToString());
		}

	}
}
