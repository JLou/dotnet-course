using FestivalPlanner;

namespace FestivalPlanner.Tests;

public class LineupParserTests
{
    [Test]
    public async Task Parse_ReadsAndTrimsAValidLine()
    {
        var set = LineupParser.Parse(
            "  Mainstage ; Volta ; 21:15 ; 22:30  ",
            TestData.FestivalDay);

        await Assert.That(set.Stage).IsEqualTo("Mainstage");
        await Assert.That(set.Artist).IsEqualTo("Volta");
        await Assert.That(set.Start).IsEqualTo(TestData.At("2026-07-11 21:15"));
        await Assert.That(set.End).IsEqualTo(TestData.At("2026-07-11 22:30"));
    }

    [Test]
    [Arguments(
        "23:00",
        "00:30",
        "2026-07-11 23:00",
        "2026-07-12 00:30")]
    [Arguments(
        "00:15",
        "02:00",
        "2026-07-12 00:15",
        "2026-07-12 02:00")]
    [Arguments(
        "18:00",
        "19:00",
        "2026-07-11 18:00",
        "2026-07-11 19:00")]
    public async Task Parse_ResolvesTimesAroundMidnight(
        string start,
        string end,
        string expectedStart,
        string expectedEnd)
    {
        var set = LineupParser.Parse(
            $"Mainstage;Kaleido;{start};{end}",
            TestData.FestivalDay);

        await Assert.That(set.Start).IsEqualTo(TestData.At(expectedStart));
        await Assert.That(set.End).IsEqualTo(TestData.At(expectedEnd));
    }

    [Test]
    [Arguments("05:59", "2026-07-12 05:59")]
    [Arguments("06:00", "2026-07-11 06:00")]
    public async Task Parse_UsesSixOClockAsTheNightCutoff(
        string start,
        string expectedStart)
    {
        var set = LineupParser.Parse(
            $"Mainstage;Last DJ;{start};07:00",
            TestData.FestivalDay);

        await Assert.That(set.Start).IsEqualTo(TestData.At(expectedStart));
    }

    [Test]
    [MethodDataSource(nameof(InvalidLines))]
    public async Task Parse_RejectsInvalidLines(string line)
    {
        var action = () => LineupParser.Parse(line, TestData.FestivalDay);

        await Assert.That(action).Throws<FormatException>();
    }
    
    public static IEnumerable<string> InvalidLines()
    {
        yield return "Mainstage;Volta;21:15";
        yield return "Mainstage;Volta;21:15;22:30;extra";
        yield return ";Volta;21:15;22:30";
        yield return "Mainstage;;21:15;22:30";
        yield return "Mainstage;Volta;9pm;22:30";
        yield return "Mainstage;Volta;21:15;25:00";
    }

    [Test]
    public async Task TryParse_ReturnsFalseAndNullForAnInvalidLine()
    {
        var parsed = LineupParser.TryParse(
            "not a lineup line",
            TestData.FestivalDay,
            out var set);

        await Assert.That(parsed).IsFalse();
        await Assert.That(set).IsNull();
    }

    [Test]
    public async Task TryParse_ReturnsFalseAndNullForNull()
    {
        var parsed = LineupParser.TryParse(
            null,
            TestData.FestivalDay,
            out var set);

        await Assert.That(parsed).IsFalse();
        await Assert.That(set).IsNull();
    }

    [Test]
    public async Task ParseMany_SkipsBlankLinesAndComments()
    {
        string[] lines =
        [
            "# stage;artist;start;end",
            "",
            "   ",
            "Mainstage;Volta;21:15;22:30",
            "  # the headliner follows",
            "Mainstage;Kaleido;23:00;00:30"
        ];

        var sets = LineupParser.ParseMany(lines, TestData.FestivalDay);

        await Assert.That(sets).Count().IsEqualTo(2);
        await Assert.That(sets.Select(set => set.Artist))
            .IsEquivalentTo(["Volta", "Kaleido"]);
    }

    [Test]
    public async Task ParseMany_ReportsTheInvalidLineNumber()
    {
        string[] lines =
        [
            "# comment",
            "Mainstage;Volta;21:15;22:30",
            "",
            "invalid"
        ];

        var action = () => LineupParser.ParseMany(
            lines,
            TestData.FestivalDay);

        await Assert.That(action)
            .Throws<FormatException>()
            .WithMessageContaining("Line 4");
    }
}
