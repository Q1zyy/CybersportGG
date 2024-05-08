using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
		public EventController(IEventService eventService, ITeamService teamService)
		{
			_eventService = eventService;
			_teamService = teamService;
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

	}
}
