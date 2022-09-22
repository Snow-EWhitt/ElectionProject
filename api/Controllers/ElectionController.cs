using election.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
    }
}
