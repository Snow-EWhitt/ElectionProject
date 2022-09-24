using Microsoft.AspNetCore.Mvc.ActionConstraints;

namespace WebApp
{
    public class ApiService
    {
        public readonly IConfiguration config;
        public readonly HttpClient http;
        public readonly string apiHost;

        public ApiService(IConfiguration config, HttpClient http)
        {
            this.config = config;
            this.http = http;
            apiHost = config["apiAddress"];
        }

        public async Task<bool> IsOpenAsync()
        {
            var url = $"{apiHost}/api/election/1/isopen";
            var isOpen = await http.GetStringAsync(url);
            return isOpen == "true";
        }

        public async Task OpenBallotingAsync()
        {
            var url = $"{apiHost}/api/election/1/open";
            await http.GetStringAsync(url);
        }

        public async Task CloseBallotingAsync()
        {
            var url = $"{apiHost}/api/election/1/close";
            await http.GetStringAsync(url);
        }

        public async Task AddBallotAsync(int electionId = 1)
        {
            var url = $"{apiHost}/api/election/{electionId}/ballot?precinct=Column";
            await http.GetStringAsync(url);
        }
    }
}
