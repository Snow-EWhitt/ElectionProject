using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using shared;

namespace WebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ApiService apiService;

        public IndexModel(ILogger<IndexModel> logger, ApiService apiService)
        {
            _logger = logger;
            this.apiService = apiService;
        }

        public IEnumerable<Ballot>? Ballots { get; private set; }
        public bool IsOpen { get; set; }

        public async Task OnGet()
        {
            IsOpen = await apiService.IsOpenAsync();
        }

        public async Task<IActionResult> OnPostOpenBalloting()
        {
            await apiService.OpenBallotingAsync();
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostCloseBalloting()
        {
            await apiService.CloseBallotingAsync();
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostAddBallot()
        {
            await apiService.AddBallotAsync();
            return RedirectToPage();
        }
    }
}