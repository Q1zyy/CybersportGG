using Microsoft.AspNetCore.Mvc;
using WebApplication1.Services;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
	public class MatchesController : Controller
	{

		private readonly IMatchService _matchService;
		private readonly ITeamService _teamService;
		private readonly IEventService _eventService;

		public MatchesController(
			IMatchService matchService,
			ITeamService teamService,
			IEventService eventService
		)
		{
			_matchService = matchService;
			_teamService = teamService;
			_eventService = eventService;
		}

		[HttpGet("/matches")]
		public async Task<IActionResult> Matches()
		{
			MatchesPageViewModel viewModel = new MatchesPageViewModel();
			var matches = await _matchService.GetUpcomingMatches();
			List<MatchPageViewModel> matchesFull = new List<MatchPageViewModel>();
			foreach (var match in matches)
			{
				var team1 = await _teamService.GetTeam(match.Team1);
				var team2 = await _teamService.GetTeam(match.Team2);
				matchesFull.Add(new MatchPageViewModel
				{
					Date = match.Date,
					Team1 = team1,
					Team2 = team2,
					EventName = await _eventService.GetEvent(match.EventId)
				}) ;
			}
			viewModel.Matches = matchesFull;
			return View(viewModel);
		}

	}
}
