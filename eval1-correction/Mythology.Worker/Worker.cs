namespace WorkerService1;

using Mythology.Core;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly ITournamentProvider _tournamentProvider;
    private readonly IRules _rules;

    public Worker(ILogger<Worker> logger, ITournamentProvider tournamentProvider, IRules rules)
    {
        _logger = logger;
        _tournamentProvider = tournamentProvider;
        _rules = rules;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var victoires = new Dictionary<string, int>();

        var matches = await _tournamentProvider.GetMatches();

        if (matches != null)
        {
            foreach (var match in matches)
            {
                var winner = _rules.GetWinner(match);
                if (!victoires.TryAdd(winner, 1))
                {
                    victoires[winner]++;
                }
            }
        }

        var tournamentwinner = victoires.Where(i => i.Key != $"Draw").MaxBy(i => i.Value);

        _logger.LogInformation(
            "Le gagnant du tournoi est {Winner} avec {Score} victoires",
            tournamentwinner.Key,
            tournamentwinner.Value
        );
    }
}
