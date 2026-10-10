namespace FestivalPlanner;

/// <summary>
/// Why two sets of a plan cannot both be watched.
/// </summary>
public enum ClashKind
{
    /// <summary>The two artists are on stage at the same time.</summary>
    Overlap,

    /// <summary>The sets follow each other too closely to walk from one stage to the other.</summary>
    NotEnoughTimeToWalk
}

/// <summary>
/// Two sets of a plan that cannot both be watched, earliest set first.
/// </summary>
public sealed record Clash(FestivalSet First, FestivalSet Second, ClashKind Kind, TimeSpan Overlap)
{
    public override string ToString() =>
        Kind == ClashKind.Overlap
            ? $"{First.Artist} and {Second.Artist} overlap by {Overlap.TotalMinutes:0} min"
            : $"{First.Artist} -> {Second.Artist}: not enough time to reach {Second.Stage}";
}

/// <summary>
/// Finds the clashes in a personal plan, taking into account the time needed to walk between stages.
/// </summary>
public sealed class ClashDetector
{
    private readonly TimeSpan _walkingTime;

    public ClashDetector() : this(TimeSpan.Zero)
    {
    }

    public ClashDetector(TimeSpan walkingTime)
    {
        if (walkingTime < TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(nameof(walkingTime), "Walking time cannot be negative.");
        }

        _walkingTime = walkingTime;
    }

    /// <summary>
    /// Free time between the end of the earlier set and the start of the later one.
    /// Zero when the two sets overlap.
    /// </summary>
    public static TimeSpan GapBetween(FestivalSet first, FestivalSet second)
    {
        ArgumentNullException.ThrowIfNull(first);
        ArgumentNullException.ThrowIfNull(second);

        if (first.Overlaps(second))
        {
            return TimeSpan.Zero;
        }

        var (earlier, later) = Order(first, second);
        return later.Start - earlier.End;
    }

    /// <summary>
    /// Tells whether both sets can be watched from beginning to end.
    /// </summary>
    public bool CanWatchBoth(FestivalSet first, FestivalSet second)
    {
        ArgumentNullException.ThrowIfNull(first);
        ArgumentNullException.ThrowIfNull(second);

        if (first.Overlaps(second))
        {
            return false;
        }

        if (string.Equals(first.Stage, second.Stage, StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        return GapBetween(first, second) > _walkingTime;
    }

    /// <summary>
    /// Every pair of the plan that cannot both be watched, earliest clash first.
    /// </summary>
    public IReadOnlyList<Clash> FindClashes(IEnumerable<FestivalSet> plan)
    {
        ArgumentNullException.ThrowIfNull(plan);

        var sets = new List<FestivalSet>(plan);
        var clashes = new List<Clash>();

        for (var i = 0; i < sets.Count; i++)
        {
            for (var j = i + 1; j < sets.Count; j++)
            {
                if (CanWatchBoth(sets[i], sets[j]))
                {
                    continue;
                }

                var (earlier, later) = Order(sets[i], sets[j]);
                var overlap = earlier.OverlapWith(later);
                var kind = overlap > TimeSpan.Zero ? ClashKind.Overlap : ClashKind.NotEnoughTimeToWalk;
                clashes.Add(new Clash(earlier, later, kind, overlap));
            }
        }

        clashes.Sort(static (left, right) => left.First.Start.CompareTo(right.First.Start));
        return clashes;
    }

    private static (FestivalSet Earlier, FestivalSet Later) Order(FestivalSet first, FestivalSet second) =>
        first.Start <= second.Start ? (first, second) : (second, first);
}
