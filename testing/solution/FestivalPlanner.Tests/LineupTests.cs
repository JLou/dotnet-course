using FestivalPlanner;

namespace FestivalPlanner.Tests;

public class LineupTests
{
    [Test]
    public async Task Add_AppendsASet()
    {
        var lineup = new Lineup();
        var set = TestData.Set("Volta", "Mainstage", "21:15", "22:30");

        lineup.Add(set);

        await Assert.That(lineup.Count).IsEqualTo(1);
        await Assert.That(lineup.Sets).Contains(set);
    }

    [Test]
    public async Task Add_RejectsOverlappingSetsOnTheSameStage()
    {
        var lineup = new Lineup();
        lineup.Add(TestData.Set("Volta", "Mainstage", "21:15", "22:30"));

        var action = () => lineup.Add(
            TestData.Set("Other Artist", "mainstage", "22:00", "23:00"));

        await Assert.That(action).Throws<InvalidOperationException>();
    }

    [Test]
    public async Task Add_AcceptsBackToBackSetsOnTheSameStage()
    {
        var lineup = new Lineup();
        lineup.Add(TestData.Set("Volta", "Mainstage", "21:15", "22:30"));

        lineup.Add(TestData.Set("Kaleido", "Mainstage", "22:30", "23:30"));

        await Assert.That(lineup.Count).IsEqualTo(2);
    }

    [Test]
    public async Task Add_AcceptsOverlappingSetsOnDifferentStages()
    {
        var lineup = new Lineup();
        lineup.Add(TestData.Set("Volta", "Mainstage", "21:15", "22:30"));

        lineup.Add(TestData.Set("Blunt Theory", "Warehouse", "21:30", "23:00"));

        await Assert.That(lineup.Count).IsEqualTo(2);
    }

    [Test]
    public async Task Add_RejectsTheSameArtistTwiceIgnoringCase()
    {
        var lineup = new Lineup();
        lineup.Add(TestData.Set("Volta", "Mainstage", "21:15", "22:30"));

        var action = () => lineup.Add(
            TestData.Set("VOLTA", "Warehouse", "23:00", "00:30"));

        await Assert.That(action).Throws<InvalidOperationException>();
    }

    [Test]
    public async Task PlayingAt_ReturnsEveryArtistPlayingAtThatMoment()
    {
        var playing = TestData.Lineup().PlayingAt(
            TestData.At("2026-07-11 21:40"));

        await Assert.That(playing.Select(set => set.Artist))
            .IsEquivalentTo(["Oiseau Bleu", "Volta", "Blunt Theory"]);
    }

    [Test]
    public async Task PlayingAt_ReturnsAnEmptyCollectionBetweenSets()
    {
        var playing = TestData.Lineup().PlayingAt(
            TestData.At("2026-07-11 20:00"));

        await Assert.That(playing).IsEmpty();
    }

    [Test]
    public async Task ForStage_IgnoresCaseAndOrdersSetsByStart()
    {
        var sets = TestData.Lineup().ForStage("MAINSTAGE");

        await Assert.That(sets).Count().IsEqualTo(2);
        await Assert.That(sets[0].Artist).IsEqualTo("Volta");
        await Assert.That(sets[1].Artist).IsEqualTo("Kaleido");
    }

    [Test]
    public async Task Find_IgnoresArtistCase()
    {
        var set = TestData.Lineup().Find("oIsEaU bLeU");

        await Assert.That(set).IsNotNull();
        await Assert.That(set!.Artist).IsEqualTo("Oiseau Bleu");
    }

    [Test]
    public async Task Find_ReturnsNullForAnUnknownArtist()
    {
        await Assert.That(TestData.Lineup().Find("Daft Punk")).IsNull();
    }

    [Test]
    public async Task Stages_AreUniqueCaseInsensitiveAndAlphabetical()
    {
        var lineup = TestData.Lineup();

        await Assert.That(lineup.Stages).Count().IsEqualTo(3);
        await Assert.That(lineup.Stages[0]).IsEqualTo("Greenhouse");
        await Assert.That(lineup.Stages[1]).IsEqualTo("Mainstage");
        await Assert.That(lineup.Stages[2]).IsEqualTo("Warehouse");
    }

    [Test]
    public async Task Headliners_ReturnTheLastSetOfEachStage()
    {
        var headliners = TestData.Lineup().Headliners();

        await Assert.That(headliners.Select(set => set.Artist))
            .IsEquivalentTo(["Aube", "Kaleido", "Blunt Theory"]);
    }

    [Test]
    public async Task InStartOrder_ReturnsSetsChronologically()
    {
        var sets = TestData.Lineup().InStartOrder();

        await Assert.That(sets).Count().IsEqualTo(5);
        await Assert.That(sets[0].Artist).IsEqualTo("Oiseau Bleu");
        await Assert.That(sets[1].Artist).IsEqualTo("Volta");
        await Assert.That(sets[2].Artist).IsEqualTo("Blunt Theory");
        await Assert.That(sets[3].Artist).IsEqualTo("Kaleido");
        await Assert.That(sets[4].Artist).IsEqualTo("Aube");
    }
}
