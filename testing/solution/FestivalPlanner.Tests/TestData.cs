using System.Globalization;
using FestivalPlanner;

namespace FestivalPlanner.Tests;

internal static class TestData
{
    internal static readonly DateOnly FestivalDay = new(2026, 7, 11);

    internal static FestivalSet Set(
        string artist,
        string stage,
        string start,
        string end) =>
        LineupParser.Parse($"{stage};{artist};{start};{end}", FestivalDay);

    internal static DateTime At(string dateAndTime) =>
        DateTime.ParseExact(
            dateAndTime,
            "yyyy-MM-dd HH:mm",
            CultureInfo.InvariantCulture);

    internal static Lineup Lineup()
    {
        var lineup = new Lineup();
        lineup.Add(Set("Kaleido", "Mainstage", "23:00", "00:30"));
        lineup.Add(Set("Oiseau Bleu", "Greenhouse", "20:30", "21:45"));
        lineup.Add(Set("Volta", "Mainstage", "21:15", "22:30"));
        lineup.Add(Set("Blunt Theory", "Warehouse", "21:30", "23:00"));
        lineup.Add(Set("Aube", "Greenhouse", "00:15", "02:00"));
        return lineup;
    }
}
