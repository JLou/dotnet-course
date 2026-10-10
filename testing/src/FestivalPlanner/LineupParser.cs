using System.Globalization;

namespace FestivalPlanner;

/// <summary>
/// Reads the lineup file published by the festival. One set per line:
/// <c>stage;artist;start;end</c>, times written as <c>HH:mm</c>.
/// Blank lines and lines starting with <c>#</c> are ignored.
/// </summary>
/// <remarks>
/// Times have no date: they are resolved against the festival day. A set announced before
/// <see cref="NightEndsAt"/> belongs to the night of that day, so it is resolved on the next
/// calendar day. A set whose end time is not after its start time also rolls over to the next day.
/// </remarks>
public static class LineupParser
{
    private const char Separator = ';';
    private const string TimeFormat = "HH:mm";

    /// <summary>
    /// Times before this hour belong to the night of the festival day, not to its morning.
    /// </summary>
    public static readonly TimeOnly NightEndsAt = new(6, 0);

    /// <summary>
    /// Parses a single lineup line against the given festival day.
    /// </summary>
    /// <exception cref="FormatException">The line is not a valid set declaration.</exception>
    public static FestivalSet Parse(string line, DateOnly festivalDay)
    {
        ArgumentNullException.ThrowIfNull(line);

        var parts = line.Split(Separator);
        if (parts.Length != 4)
        {
            throw new FormatException(
                $"Expected 'stage{Separator}artist{Separator}start{Separator}end' but got '{line}'.");
        }

        var stage = parts[0].Trim();
        if (stage.Length == 0)
        {
            throw new FormatException($"The stage is missing in '{line}'.");
        }

        var artist = parts[1].Trim();
        if (artist.Length == 0)
        {
            throw new FormatException($"The artist is missing in '{line}'.");
        }

        var startTime = ParseTime(parts[2], line);
        var endTime = ParseTime(parts[3], line);

        var start = Resolve(startTime, festivalDay);
        var end = DateOnly.FromDateTime(start).ToDateTime(endTime);
        if (end <= start)
        {
            end = end.AddDays(1);
        }

        return new FestivalSet(artist, stage, start, end);
    }

    /// <summary>
    /// Parses a lineup line without throwing.
    /// </summary>
    public static bool TryParse(string? line, DateOnly festivalDay, out FestivalSet? set)
    {
        set = null;
        if (line is null)
        {
            return false;
        }

        try
        {
            set = Parse(line, festivalDay);
            return true;
        }
        catch (FormatException)
        {
            return false;
        }
    }

    /// <summary>
    /// Parses a whole lineup file, skipping blank lines and comments.
    /// </summary>
    /// <exception cref="FormatException">One of the set declarations is invalid.</exception>
    public static IReadOnlyList<FestivalSet> ParseMany(IEnumerable<string> lines, DateOnly festivalDay)
    {
        ArgumentNullException.ThrowIfNull(lines);

        var sets = new List<FestivalSet>();
        var lineNumber = 0;

        foreach (var line in lines)
        {
            lineNumber++;
            if (IsIgnored(line))
            {
                continue;
            }

            try
            {
                sets.Add(Parse(line, festivalDay));
            }
            catch (FormatException exception)
            {
                throw new FormatException($"Line {lineNumber}: {exception.Message}", exception);
            }
        }

        return sets;
    }

    private static bool IsIgnored(string line) =>
        string.IsNullOrWhiteSpace(line) || line.TrimStart().StartsWith('#');

    private static DateTime Resolve(TimeOnly time, DateOnly festivalDay)
    {
        var day = time < NightEndsAt ? festivalDay.AddDays(1) : festivalDay;
        return day.ToDateTime(time);
    }

    private static TimeOnly ParseTime(string value, string line)
    {
        if (!TimeOnly.TryParseExact(value.Trim(), TimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out var time))
        {
            throw new FormatException($"'{value.Trim()}' is not a time written as {TimeFormat} in '{line}'.");
        }

        return time;
    }
}
