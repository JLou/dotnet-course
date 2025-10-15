namespace Mythology.Core;

public class Match
{
    public Player Joueur1 { get; set; }
    public Player Joueur2 { get; set; }
}

public class Player {
    public string Nom { get; set; }
    public IList<string> Moves { get; set; }
    
}
