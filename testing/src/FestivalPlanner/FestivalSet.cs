namespace FestivalPlanner;

/// <summary>
/// One artist playing on one stage, from <see cref="Start"/> (included) to <see cref="End"/> (excluded).
/// </summary>
public sealed record FestivalSet
{
    public FestivalSet(string artist, string stage, DateTime start, DateTime end)
    {
        if (string.IsNullOrWhiteSpace(artist))
        {
            throw new ArgumentException("A set must have an artist.", nameof(artist));
        }

        if (string.IsNullOrWhiteSpace(stage))
        {
            throw new ArgumentException("A set must have a stage.", nameof(stage));
        }

        if (end <= start)
        {
            throw new ArgumentException("A set must end after it starts.", nameof(end));
        }

        Artist = artist.Trim();
        Stage = stage.Trim();
        Start = start;
        End = end;
    }

    public string Artist { get; }

    public string Stage { get; }

    public DateTime Start { get; }

    public DateTime End { get; }

    public TimeSpan Duration => End - Start;

    /// <summary>
    /// Tells whether the artist is on stage at that exact moment.
    /// </summary>
    public bool IsPlayingAt(DateTime moment) => moment >= Start && moment < End;

    /// <summary>
    /// Tells whether the two sets are on stage at the same time.
    /// </summary>
    public bool Overlaps(FestivalSet other)
    {
        ArgumentNullException.ThrowIfNull(other);
        return Start < other.End && other.Start < End;
    }

    /// <summary>
    /// How much of the two sets happens at the same time. Zero when they do not overlap.
    /// </summary>
    public TimeSpan OverlapWith(FestivalSet other)
    {
        ArgumentNullException.ThrowIfNull(other);

        var start = Start > other.Start ? Start : other.Start;
        var end = End < other.End ? End : other.End;
        return end > start ? end - start : TimeSpan.Zero;
    }

    public override string ToString() => $"{Start:HH:mm}-{End:HH:mm} {Artist} ({Stage})";
}
