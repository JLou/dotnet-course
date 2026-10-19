using FestivalPlanner;

namespace FestivalPlanner.Tests;

public class FestivalSetTests
{
    [Test]
    public async Task Constructor_TrimsArtistAndStage()
    {
        var set = new FestivalSet(
            "  Volta  ",
            "  Mainstage  ",
            TestData.At("2026-07-11 21:15"),
            TestData.At("2026-07-11 22:30"));

        await Assert.That(set.Artist).IsEqualTo("Volta");
        await Assert.That(set.Stage).IsEqualTo("Mainstage");
    }

    [Test]
    [Arguments("")]
    [Arguments(" ")]
    [Arguments("   ")]
    public async Task Constructor_RejectsMissingArtist(string artist)
    {
        var action = () => new FestivalSet(
            artist,
            "Mainstage",
            TestData.At("2026-07-11 21:15"),
            TestData.At("2026-07-11 22:30"));

        await Assert.That(action).Throws<ArgumentException>();
    }

    [Test]
    [Arguments("")]
    [Arguments(" ")]
    [Arguments("   ")]
    public async Task Constructor_RejectsMissingStage(string stage)
    {
        var action = () => new FestivalSet(
            "Volta",
            stage,
            TestData.At("2026-07-11 21:15"),
            TestData.At("2026-07-11 22:30"));

        await Assert.That(action).Throws<ArgumentException>();
    }

    [Test]
    [Arguments("2026-07-11 21:15")]
    [Arguments("2026-07-11 20:00")]
    public async Task Constructor_RejectsAnEndThatIsNotAfterTheStart(string end)
    {
        var action = () => new FestivalSet(
            "Volta",
            "Mainstage",
            TestData.At("2026-07-11 21:15"),
            TestData.At(end));

        await Assert.That(action).Throws<ArgumentException>();
    }

    [Test]
    public async Task Duration_IsTheTimeOnStage()
    {
        // var set = TestData.Set("Volta", "Mainstage", "21:15", "22:30");

        var set = new FestivalSet(
            "Volta",
            "Mainstage",
            new DateTime(2026, 7, 11, 21, 15, 0),
            new DateTime(2026, 7, 11, 22, 30, 0));
        await Assert.That(set.Duration).IsEqualTo(TimeSpan.FromMinutes(75));
    }

    [Test]
    [Arguments("2026-07-11 21:14", false)]
    [Arguments("2026-07-11 21:15", true)]
    [Arguments("2026-07-11 22:29", true)]
    [Arguments("2026-07-11 22:30", false)]
    public async Task IsPlayingAt_IncludesTheStartButExcludesTheEnd(
        string moment,
        bool expected)
    {
        var set = TestData.Set("Volta", "Mainstage", "21:15", "22:30");

        await Assert.That(set.IsPlayingAt(TestData.At(moment))).IsEqualTo(expected);
    }

    [Test]
    [Arguments("21:00", "21:15", false)]
    [Arguments("22:30", "23:00", false)]
    [Arguments("22:29", "23:00", true)]
    [Arguments("21:00", "21:16", true)]
    [Arguments("21:30", "22:00", true)]
    public async Task Overlaps_DetectsSharedTime(
        string otherStart,
        string otherEnd,
        bool expected)
    {
        var set = TestData.Set("Volta", "Mainstage", "21:15", "22:30");
        var other = TestData.Set("Other Artist", "Warehouse", otherStart, otherEnd);

        await Assert.That(set.Overlaps(other)).IsEqualTo(expected);
    }

    [Test]
    public async Task OverlapWith_ReturnsTheSharedDuration()
    {
        var volta = TestData.Set("Volta", "Mainstage", "21:15", "22:30");
        var bluntTheory = TestData.Set(
            "Blunt Theory",
            "Warehouse",
            "21:30",
            "23:00");

        await Assert.That(volta.OverlapWith(bluntTheory))
            .IsEqualTo(TimeSpan.FromMinutes(60));
    }

    [Test]
    public async Task OverlapWith_ReturnsZeroForBackToBackSets()
    {
        var first = TestData.Set("Volta", "Mainstage", "21:15", "22:30");
        var second = TestData.Set("Kaleido", "Mainstage", "22:30", "23:30");

        await Assert.That(first.OverlapWith(second)).IsEqualTo(TimeSpan.Zero);
    }
}
