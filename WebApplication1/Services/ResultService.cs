using WebApplication1.Models;

namespace WebApplication1.Services
{
    public class ResultService : IResultService
    {
        private ApplicationDbContext db;

        public ResultService(ApplicationDbContext context)
        {
            db = context;
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
    }
}
