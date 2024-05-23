using Microsoft.AspNetCore.Mvc;
using WebApplication1.Services;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
	public class ResultsController : Controller
	{

		private readonly IMatchService _matchService;
		private readonly ITeamService _teamService;
		private readonly IEventService _eventService;
		private readonly IResultService _resultService;

		public ResultsController(
			IMatchService matchService,
			ITeamService teamService,
			IEventService eventService,
			IResultService resultService
		)
		{
			_matchService = matchService;
			_teamService = teamService;
			_eventService = eventService;
			_resultService = resultService;
		}

		[HttpGet("/results")]
		public async Task<ActionResult> Results([FromQuery] int? teamId = null)
		{

			ResultsPageViewModel viewModel = new ResultsPageViewModel();
			List<ResultPageViewModel> results = new List<ResultPageViewModel>();
			var res = _resultService.GetResults().Result;

			if (teamId.HasValue)
			{
				res = await _resultService.GetTeamResults(teamId.Value);
			}

			foreach (var r in res)
			{
				var match = await _matchService.GetMatch(r.MatchId);
				var team1 = await _teamService.GetTeam(match.Team1);
				var team2 = await _teamService.GetTeam(match.Team2);
				var ev = await _eventService.GetEvent(match.EventId);
				results.Add(new ResultPageViewModel
				{
					Team1 = team1,
					Team2 = team2,
					Score = r.Score,
					Winner = r.Winner,
					Date = match.Date,
					Event = ev,
					Id = match.Id
				});
			}
			viewModel.Results = results;
			return View(viewModel);
		}

	}
}
