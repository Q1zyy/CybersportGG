using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Identity.Client;
using System.Net.WebSockets;
using System.Security.Claims;
using WebApplication1.Models;
using WebApplication1.Services;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
	public class EventController : Controller
	{

		private readonly IEventService _eventService;
		private readonly ITeamService _teamService;
		private readonly IMatchService _matchService;
		public EventController(
			IEventService eventService,
			ITeamService teamService,
			IMatchService matchService
		)
		{
			_eventService = eventService;
			_teamService = teamService;
			_matchService = matchService;
		}

		[HttpGet]
		[Authorize(Policy = "admin")]
		public async Task<IActionResult> CreateEvent()
		{
			return View();
		}	
		
		[HttpPost]
		[Authorize(Policy = "admin")]
		public async Task<IActionResult> CreateEvent(EventViewModel model)
		{
			if (ModelState.IsValid)
			{

				byte[] bytes = null;
				using (MemoryStream memoryStream = new MemoryStream())
				{
					await model.Image.CopyToAsync(memoryStream);
					bytes = memoryStream.ToArray();
				}

				await _eventService.AddEvent(new Models.Event()
				{
					Name = model.Name,
					StartDate = model.StartDate,
					EndDate = model.EndDate,
					Image = bytes
				});
			}
			return View();
		}

		[HttpGet("/events")]
		public async Task<IActionResult> Events()
		{
			var events = await _eventService.GetOngoingEvents();
			ViewBag.Events = events;
			ViewBag.Role = User.Claims.Where(x => x.Type == ClaimTypes.Role).Select(c => c.Value).SingleOrDefault();
			return View();
		}


		[HttpGet("/event/{id:int}")]
		public async Task<IActionResult> Event(int id)
		{
			List<Team> teams = new List<Team>();
			List<MatchViewModelFull> mathes = new List<MatchViewModelFull>();
			Event curEvent = await _eventService.GetEvent(id);
			if (curEvent != null)
			{
				foreach (var i in curEvent.TeamsId)
				{
					teams.Add(await _teamService.GetTeam(i));
				}

				foreach (var i in curEvent.MatchesId)
				{
					var match = await _matchService.GetMatch(i);
					mathes.Add(new MatchViewModelFull()
					{
						Date = match.Date,
						Team1 = await _teamService.GetTeam(match.Team1),
						Team2 = await _teamService.GetTeam(match.Team2)
					});
				}

			}
			ViewBag.CurrentEvent = curEvent;
			ViewBag.CurrentTeams = teams;
			ViewBag.CurrentMatches = mathes;
			ViewBag.Role = User.Claims.Where(x => x.Type == ClaimTypes.Role).Select(c => c.Value).SingleOrDefault();
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> SearchTeams(int id)
		{
			List<Team> teams = new List<Team>();
			Event curEvent = await _eventService.GetEvent(id);
			ViewBag.CurrentEvent = curEvent;
			if (curEvent != null)
			{
				foreach (var i in curEvent.TeamsId)
				{
					teams.Add(await _teamService.GetTeam(i));
				}
			}
			ViewBag.CurrentTeams = teams;
			ViewBag.Role = User.Claims.Where(x => x.Type == ClaimTypes.Role).Select(c => c.Value).SingleOrDefault();
			string str = Request.Form["searchString"];
			ViewBag.SearchingTeams = await _teamService.SearchTeams(str);
			return View("Event");
		}

		public async Task<IActionResult> AddTeam(int id, int teamId)
		{
			await _eventService.AddTeam(id, teamId);
			return Redirect(id.ToString());
		}	
		
		public async Task<IActionResult> DeleteTeam(int id, int teamId)
		{
			await _eventService.DeleteTeam(id, teamId);
			return Redirect(id.ToString());
		}

		[HttpGet]
		public async Task<IActionResult> AddMatch(int id)
		{
			Event curEvent = await _eventService.GetEvent(id);
			ViewBag.CurrentEvent = curEvent;
			var teams = (await _eventService.GetEvent(id)).TeamsId;
			List<Team> teams1 = new List<Team>();
			var options = new List<SelectListItem>();
			foreach (var team in teams)
			{
				teams1.Add(await _teamService.GetTeam(team));
			}
			foreach (var team in teams1)
			{
				options.Add(new SelectListItem
				{
					Value = team.Name,
					Text = team.Name
				});
			}
			var model = new MatchViewModel
			{
				Options = options
			};
			ViewBag.CurrentTeams = teams1;

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> AddMatch(int id, MatchViewModel model)
		{
			var ev = await _eventService.GetEvent(id);
			var t1 = await _teamService.GetTeamByName(model.Team1);
			var t2 = await _teamService.GetTeamByName(model.Team2);
			var match = new Match()
			{
				Date = model.Date,
				EventId = id,
				Team1 = t1.Id,
				Team2 = t2.Id
			};
			var m = await _matchService.AddMatch(match);
			await _eventService.AddMatchToEvent(id, m.Id);
			return Redirect(id.ToString());
		}

	}
}
