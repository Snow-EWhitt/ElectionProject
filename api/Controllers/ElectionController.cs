using election.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace election.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ElectionController : ControllerBase
    {
        private readonly InstantRunoffContext context;

        public ElectionController(InstantRunoffContext context)
        {
            this.context = context;
        }

        [HttpGet("{electionId}/isopen")]
        public async Task<string> IsOpen(int electionId)
        {
            var row = await context.Elections.FirstOrDefaultAsync(e => e.Id == electionId);
            return row.Ballotingclosed ? "false" : "true";
        }

        [HttpGet("{electionId}/open")]
        public async Task OpenBalloting(int electionId)
        {
            var row = await context.Elections.FirstOrDefaultAsync(e => e.Id == electionId);
            row.Ballotingclosed = false;
            await context.SaveChangesAsync();
        }

        [HttpGet("{electionId}/close")]
        public async Task CloseBalloting(int electionId)
        {
            var row = await context.Elections.FirstOrDefaultAsync(e => e.Id == electionId);
            row.Ballotingclosed = true;
            await context.SaveChangesAsync();
        }

        [HttpGet("{electionId}/ballot")]
        public async Task AddBallot(int electionId, string precinct)
        {
            try
            {
                await context.Database.ExecuteSqlRawAsync("call sim_ballots_bulk(@election, 10000, 1000, @precinct)",
                    new NpgsqlParameter("election", electionId),
                    new NpgsqlParameter("precinct", precinct)
                );
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
