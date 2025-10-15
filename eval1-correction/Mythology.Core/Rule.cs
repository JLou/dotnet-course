namespace Mythology.Core;

public class Rules : IRules
{
    private readonly IDictionary<string, ISet<string>> _rules = new Dictionary<string, ISet<string>>()
    {
        ["Zeus"] = new HashSet<string>() { "Ares", "Poseidon" },
        ["Athena"] = new HashSet<string>() { "Zeus", "Ares" },
        ["Hades"] = new HashSet<string>() { "Zeus", "Artemis" },
        ["Ares"] = new HashSet<string>() { "Hades", "Artemis" },
        ["Poseidon"] = new HashSet<string>() { "Athena", "Hades" },
        ["Artemis"] = new HashSet<string>() { "Poseidon", "Athena" },
    };
    
    public bool Beats(string god1, string god2)
    {
        return _rules.ContainsKey(god1) && _rules[god1].Contains(god2);
    }

    public string GetWinner(Match match)
    {
        var player1 = match.Joueur1;
        var player2 = match.Joueur2;
        var score1 = 0;
        var score2 = 0;

        foreach (var (firstGod, secondGod) in match.Joueur1.Moves.Zip(match.Joueur2.Moves))
        {
            if (Beats(firstGod, secondGod))
            {
                score1++;
            }
            else if (Beats(secondGod, firstGod))
            {
                score2++;
            }
        }
        
        if (score1 > score2)
        {
            return player1.Nom;
        }

        return score2 > score1 ? player2.Nom : "Draw";
    }
}

public interface IRules
{
    bool Beats(string god1, string god2);
    
    string GetWinner(Match match);
}
