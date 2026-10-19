using FestivalPlanner;

namespace FestivalPlanner.Tests;

public class ClashDetectorTests
{
    [Test]
    public async Task Constructor_RejectsNegativeWalkingTime()
    {
        var action = () => new ClashDetector(TimeSpan.FromMinutes(-1));

        await Assert.That(action).Throws<ArgumentOutOfRangeException>();
    }

    [Test]
    [Arguments(0, true)]
    [Arguments(14, true)]
    [Arguments(15, true)]
    [Arguments(16, false)]
    [Arguments(30, false)]
    public async Task CanWatchBoth_DependsOnTheWalkingTime(
        int walkingMinutes,
        bool expected)
    {
        var first = TestData.Set("Volta", "Mainstage", "21:15", "22:30");
        var second = TestData.Set(
            "Deep Signal",
            "Warehouse",
            "22:45",
            "23:30");
        var detector = new ClashDetector(
            TimeSpan.FromMinutes(walkingMinutes));

        await Assert.That(detector.CanWatchBoth(first, second))
            .IsEqualTo(expected);
    }

    [Test]
    public async Task CanWatchBoth_ReturnsFalseWhenSetsOverlap()
    {
        var first = TestData.Set("Volta", "Mainstage", "21:15", "22:30");
        var second = TestData.Set(
            "Blunt Theory",
            "Warehouse",
            "21:30",
            "23:00");

        await Assert.That(new ClashDetector().CanWatchBoth(first, second))
            .IsFalse();
    }

    [Test]
    public async Task CanWatchBoth_NeedsNoWalkingTimeOnTheSameStage()
    {
        var first = TestData.Set("Volta", "Mainstage", "21:15", "22:30");
        var second = TestData.Set("Kaleido", "mainstage", "22:30", "23:30");
        var detector = new ClashDetector(TimeSpan.FromMinutes(30));

        await Assert.That(detector.CanWatchBoth(first, second)).IsTrue();
    }

    [Test]
    public async Task GapBetween_ReturnsTheTimeBetweenSetsRegardlessOfOrder()
    {
        var first = TestData.Set("Volta", "Mainstage", "21:15", "22:30");
        var second = TestData.Set(
            "Deep Signal",
            "Warehouse",
            "22:45",
            "23:30");

        await Assert.That(ClashDetector.GapBetween(first, second))
            .IsEqualTo(TimeSpan.FromMinutes(15));
        await Assert.That(ClashDetector.GapBetween(second, first))
            .IsEqualTo(TimeSpan.FromMinutes(15));
    }

    [Test]
    public async Task GapBetween_ReturnsZeroWhenSetsOverlap()
    {
        var first = TestData.Set("Volta", "Mainstage", "21:15", "22:30");
        var second = TestData.Set(
            "Blunt Theory",
            "Warehouse",
            "21:30",
            "23:00");

        await Assert.That(ClashDetector.GapBetween(first, second))
            .IsEqualTo(TimeSpan.Zero);
    }

    [Test]
    public async Task FindClashes_ReturnsBothKindsInChronologicalOrder()
    {
        FestivalSet[] plan =
        [
            TestData.Set("Kaleido", "Mainstage", "23:00", "00:30"),
            TestData.Set("Oiseau Bleu", "Greenhouse", "20:30", "21:45"),
            TestData.Set("Blunt Theory", "Warehouse", "21:30", "23:00"),
            TestData.Set("Aube", "Greenhouse", "00:15", "02:00")
        ];
        var detector = new ClashDetector(TimeSpan.FromMinutes(15));

        var clashes = detector.FindClashes(plan);

        await Assert.That(clashes).Count().IsEqualTo(3);

        await Assert.That(clashes[0].First.Artist).IsEqualTo("Oiseau Bleu");
        await Assert.That(clashes[0].Second.Artist).IsEqualTo("Blunt Theory");
        await Assert.That(clashes[0].Kind).IsEqualTo(ClashKind.Overlap);
        await Assert.That(clashes[0].Overlap)
            .IsEqualTo(TimeSpan.FromMinutes(15));

        await Assert.That(clashes[1].First.Artist).IsEqualTo("Blunt Theory");
        await Assert.That(clashes[1].Second.Artist).IsEqualTo("Kaleido");
        await Assert.That(clashes[1].Kind)
            .IsEqualTo(ClashKind.NotEnoughTimeToWalk);
        await Assert.That(clashes[1].Overlap).IsEqualTo(TimeSpan.Zero);

        await Assert.That(clashes[2].First.Artist).IsEqualTo("Kaleido");
        await Assert.That(clashes[2].Second.Artist).IsEqualTo("Aube");
        await Assert.That(clashes[2].Kind).IsEqualTo(ClashKind.Overlap);
        await Assert.That(clashes[2].Overlap)
            .IsEqualTo(TimeSpan.FromMinutes(15));
    }

    [Test]
    public async Task FindClashes_ReturnsAnEmptyCollectionForAValidPlan()
    {
        FestivalSet[] plan =
        [
            TestData.Set("Volta", "Mainstage", "21:15", "22:30"),
            TestData.Set("Kaleido", "Mainstage", "22:30", "23:30"),
            TestData.Set("Aube", "Greenhouse", "00:15", "02:00")
        ];

        var clashes = new ClashDetector(
            TimeSpan.FromMinutes(30)).FindClashes(plan);

        await Assert.That(clashes).IsEmpty();
    }
}
