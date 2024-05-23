using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public class ResultService : IResultService
    {
        private ApplicationDbContext db;
        private readonly IMatchService _matchService;

        public ResultService(ApplicationDbContext context, IMatchService matchService)
        {
            db = context;
            _matchService = matchService;
        }

        public async Task AddResult(Result model)
        {
            await db.Results.AddAsync(model);
            await db.SaveChangesAsync();
        }

        public Task<Result> GetResult(int id)
        {
            throw new NotImplementedException();
        }

		public async Task<IEnumerable<Result>> GetResults()
		{
            return db.Results.ToList(); 
		}

		public async Task<IEnumerable<Result>> GetTeamResults(int teamId)
		{
            var matches = await _matchService.GetDoneTeamMatches(teamId);
            var list = new List<Result>();
            foreach (var match in matches)
            {
                list.Add(await db.Results.SingleOrDefaultAsync(r => r.MatchId == match.Id));
            }
            return list;
		}
	}
}
