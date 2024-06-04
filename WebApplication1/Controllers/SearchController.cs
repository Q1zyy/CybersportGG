using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using WebApplication1.Services;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
	public class SearchController : Controller
	{

		private readonly IPlayerService _playerService;
		private readonly ITeamService _teamService;
		private readonly IEventService _eventService;

		public SearchController(
			IPlayerService playerService,
			ITeamService teamService,
			IEventService eventService
		)
		{
			_playerService = playerService;
			_teamService = teamService;
			_eventService = eventService;
		} 

		public IActionResult Index()
		{
			return View();
		}

		[HttpGet]
		public async Task<IActionResult> Search(string str)
		{
			var model = new SearchViewModel
			{
				Players = (await _playerService.Search(str)).ToList(),
				Teams = (await _teamService.SearchTeams(str)).ToList(),
				Events = (await _eventService.Search(str)).ToList()
			};
			return PartialView("_SearchPartial", model);
		}
	}
}
