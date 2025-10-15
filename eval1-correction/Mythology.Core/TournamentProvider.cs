namespace Mythology.Core;

using System.Net.Http.Json;

public class TournamentProvider : ITournamentProvider
{
    private readonly HttpClient _client;

    public TournamentProvider(HttpClient client)
    {
        _client = client;
    }
    public async Task<IList<Match>?> GetMatches()
    {
        var response = await _client.GetAsync($"https://raw.githubusercontent.com/JLou/dotnet-course/refs/heads/main/e1.json");
        return await response.Content.ReadFromJsonAsync<List<Match>>();
    }
}

public interface ITournamentProvider
{
    Task<IList<Match>?> GetMatches();
}
