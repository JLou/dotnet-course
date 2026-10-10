namespace FestivalPlanner;

/// <summary>
/// The full programme of one festival day.
/// </summary>
public sealed class Lineup
{
    private readonly List<FestivalSet> _sets = [];

    public IReadOnlyList<FestivalSet> Sets => _sets;

    public int Count => _sets.Count;

    /// <summary>
    /// Adds a set to the programme.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// The stage is already busy at that time, or the artist already plays that day.
    /// </exception>
    public void Add(FestivalSet set)
    {
        ArgumentNullException.ThrowIfNull(set);

        foreach (var existing in _sets)
        {
            if (Matches(existing.Stage, set.Stage) && existing.Overlaps(set))
            {
                throw new InvalidOperationException(
                    $"{set.Stage} is already busy when {set.Artist} is supposed to play.");
            }

            if (Matches(existing.Artist, set.Artist))
            {
                throw new InvalidOperationException($"{set.Artist} is already in the lineup.");
            }
        }

        _sets.Add(set);
    }

    /// <summary>
    /// Every set on stage at that moment.
    /// </summary>
    public IReadOnlyList<FestivalSet> PlayingAt(DateTime moment)
    {
        var playing = new List<FestivalSet>();
        foreach (var set in _sets)
        {
            if (set.IsPlayingAt(moment))
            {
                playing.Add(set);
            }
        }

        return playing;
    }

    /// <summary>
    /// Every set of one stage, earliest first. The stage name is case-insensitive.
    /// </summary>
    public IReadOnlyList<FestivalSet> ForStage(string stage)
    {
        ArgumentNullException.ThrowIfNull(stage);

        var sets = new List<FestivalSet>();
        foreach (var set in _sets)
        {
            if (Matches(set.Stage, stage))
            {
                sets.Add(set);
            }
        }

        sets.Sort(static (left, right) => left.Start.CompareTo(right.Start));
        return sets;
    }

    /// <summary>
    /// The set of one artist, or <c>null</c> when the artist is not playing.
    /// </summary>
    public FestivalSet? Find(string artist)
    {
        ArgumentNullException.ThrowIfNull(artist);

        foreach (var set in _sets)
        {
            if (Matches(set.Artist, artist))
            {
                return set;
            }
        }

        return null;
    }

    /// <summary>
    /// Stage names, without duplicates, in alphabetical order.
    /// </summary>
    public IReadOnlyList<string> Stages
    {
        get
        {
            var stages = new SortedSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var set in _sets)
            {
                stages.Add(set.Stage);
            }

            return [.. stages];
        }
    }

    /// <summary>
    /// The last set of every stage: the headliners.
    /// </summary>
    public IReadOnlyList<FestivalSet> Headliners()
    {
        var headliners = new List<FestivalSet>();
        foreach (var stage in Stages)
        {
            var sets = ForStage(stage);
            if (sets.Count > 0)
            {
                headliners.Add(sets[^1]);
            }
        }

        return headliners;
    }

    /// <summary>
    /// The whole programme, earliest first.
    /// </summary>
    public IReadOnlyList<FestivalSet> InStartOrder()
    {
        var ordered = new List<FestivalSet>(_sets);
        ordered.Sort(static (left, right) => left.Start.CompareTo(right.Start));
        return ordered;
    }

    private static bool Matches(string left, string right) =>
        string.Equals(left, right, StringComparison.OrdinalIgnoreCase);
}
