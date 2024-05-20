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
		private readonly IResultService _resultService;
		public EventController(
			IEventService eventService,
			ITeamService teamService,
			IMatchService matchService,
			IResultService resultService
		)
		{
			_eventService = eventService;
			_teamService = teamService;
			_matchService = matchService;
			_resultService = resultService;
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

				await _eventService.AddEvent(new Event()
				{
					Name = model.Name,
					StartDate = model.StartDate,
					EndDate = model.EndDate,
					Image = bytes
				});
			}
			return Redirect("/events");
		}

		[HttpGet]
		[Authorize(Policy = "admin")]
		public async Task<IActionResult> Edit(int id)
		{
			ViewBag.CurrentEvent = await _eventService.GetEvent(id);
			return View();
		}
		
		[HttpPost]
		[Authorize(Policy = "admin")]
		public async Task<IActionResult> Edit(int id, EventViewModel model)
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

			await _eventService.ChangeEvent(id, new Event()
			{
				Name = model.Name,
				StartDate = model.StartDate,
				EndDate = model.EndDate,
				Image = bytes
			});

			return Redirect(id.ToString());
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
					if (!match.Done)
					{
						var mvmf = new MatchViewModelFull()
						{
							Id = match.Id,
							Date = match.Date,
							Team1 = await _teamService.GetTeam(match.Team1),
							Team2 = await _teamService.GetTeam(match.Team2)
						};
						mathes.Add(mvmf);
					}
				}

			}
			ViewBag.CurrentEvent = curEvent;
			ViewBag.CurrentTeams = teams;
			ViewBag.CurrentMatches = mathes;
			ViewBag.Role = User.Claims.Where(x => x.Type == ClaimTypes.Role).Select(c => c.Value).SingleOrDefault();
			return View();
		}

		[HttpGet]
        [Authorize(Policy = "admin")]
        public async Task<IActionResult> SearchTeams(int id, string str)
		{
			TeamsListViewModel model = new TeamsListViewModel
			{
				Id = id,
				teams = (await _teamService.SearchTeams(str)).ToList()
			};
			return PartialView("_TeamsPartial", model);
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
        [Authorize(Policy = "admin")]
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
        [Authorize(Policy = "admin")]
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

		[HttpGet]
        [Authorize(Policy = "admin")]
        public async Task<IActionResult> DeleteMatch(int id, int eventId)
		{
			await _matchService.DeleteMatch(id);
			await _eventService.DeleteMatchFromEvent(eventId, id);
			return Redirect(eventId.ToString());
		}
		
		[HttpGet]
        [Authorize(Policy = "admin")]
        public async Task<IActionResult> DeleteEvent(int id)
		{
			await _eventService.DeleteEvent(id);
			return Redirect("/events");
		}

		[HttpGet]
        [Authorize(Policy = "admin")]
        public async Task<IActionResult> EditMatch(int id)
		{
			Match curMatch = await _matchService.GetMatch(id);
			int eventId = curMatch.EventId;
			Event curEvent = await _eventService.GetEvent(eventId);
			ViewBag.CurrentEvent = curEvent;
			ViewBag.CurrentMatch = curMatch;
			var teams = (await _eventService.GetEvent(eventId)).TeamsId;
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

			return View(model);
		}
		
		[HttpPost]
        [Authorize(Policy = "admin")]
        public async Task<IActionResult> EditMatch(int id, MatchViewModel matchViewModel)
		{
			Match curMatch = await _matchService.GetMatch(id);
			int eventId = curMatch.EventId;
			Event curEvent = await _eventService.GetEvent(eventId);
			var t1 = await _teamService.GetTeamByName(matchViewModel.Team1);
			var t2 = await _teamService.GetTeamByName(matchViewModel.Team2);
			var newMatch = new Match
			{
				EventId = eventId,
				Team1 = t1.Id,
				Team2 = t2.Id,
				Date = matchViewModel.Date
			};

			await _matchService.ChangeMatch(id, newMatch);

			return Redirect(eventId.ToString());
		}

		[HttpGet]
        [Authorize(Policy = "admin")]
        public async Task<IActionResult> ConfirmMatch(int id)
		{
			var match = await _matchService.GetMatch(id);
			var t1 = await _teamService.GetTeam(match.Team1);
			var t2 = await _teamService.GetTeam(match.Team2);
			var options = new List<SelectListItem>();
			options.Add(new SelectListItem
			{
				Text = t1.Name,
				Value = t1.Name
			});
			options.Add(new SelectListItem
			{
				Text = t2.Name,
				Value = t2.Name
			});
			ViewBag.CurrentMatch = match;
			var model = new ResultViewModel();
            model.Options = options;
			return View(model);
		}		
		
		[HttpPost]
        [Authorize(Policy = "admin")]
        public async Task<IActionResult> ConfirmMatch(int id, ResultViewModel model)
		{
			var team = await _teamService.GetTeamByName(model.Winner);
			await _matchService.CompleteMatch(id);
			var m = await _matchService.GetMatch(id);
			await _resultService.AddResult(new Result
			{
				MatchId = id,
				Score = model.Score,
				Winner = team.Id 
			});
			return Redirect(m.EventId.ToString());
		}



	}
}
