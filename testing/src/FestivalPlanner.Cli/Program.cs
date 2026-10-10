using System.Globalization;
using FestivalPlanner;

var festivalDay = new DateOnly(2026, 7, 11);
var lineupFile = Path.Combine(AppContext.BaseDirectory, "lineup.txt");

var at = ReadTimeArgument(args) ?? new TimeOnly(21, 30);
var walkingTime = TimeSpan.FromMinutes(ReadWalkArgument(args) ?? 15);

var lineup = new Lineup();
foreach (var set in LineupParser.ParseMany(File.ReadAllLines(lineupFile), festivalDay))
{
    lineup.Add(set);
}

Console.WriteLine($"Chroma Festival — {festivalDay:yyyy-MM-dd}");
Console.WriteLine($"{lineup.Count} sets on {lineup.Stages.Count} stages");
Console.WriteLine();

Console.WriteLine("== Programme ==");
foreach (var stage in lineup.Stages)
{
    Console.WriteLine($"  {stage}");
    foreach (var set in lineup.ForStage(stage))
    {
        Console.WriteLine($"    {set.Start:HH:mm} - {set.End:HH:mm}  {set.Artist} ({set.Duration.TotalMinutes:0} min)");
    }
}
Console.WriteLine();

Console.WriteLine("== Headliners ==");
foreach (var set in lineup.Headliners())
{
    Console.WriteLine($"  {set.Stage}: {set.Artist} at {set.Start:HH:mm}");
}
Console.WriteLine();

var moment = at < LineupParser.NightEndsAt
    ? festivalDay.AddDays(1).ToDateTime(at)
    : festivalDay.ToDateTime(at);

Console.WriteLine($"== On stage at {at:HH\\:mm} ==");
var playing = lineup.PlayingAt(moment);
if (playing.Count == 0)
{
    Console.WriteLine("  nobody, go get a drink");
}

foreach (var set in playing)
{
    Console.WriteLine($"  {set.Artist} on {set.Stage}, until {set.End:HH:mm}");
}

Console.WriteLine();

string[] wishlist = ["Neon Harbour", "Oiseau Bleu", "Blunt Theory", "Volta", "Kaleido", "Aube"];
var plan = new List<FestivalSet>();

Console.WriteLine("== My plan ==");
foreach (var artist in wishlist)
{
    var set = lineup.Find(artist);
    if (set is null)
    {
        Console.WriteLine($"  {artist} is not playing today");
        continue;
    }

    plan.Add(set);
    Console.WriteLine($"  {set}");
}

Console.WriteLine();

var detector = new ClashDetector(walkingTime);
var clashes = detector.FindClashes(plan);

Console.WriteLine($"== Clashes (allowing {walkingTime.TotalMinutes:0} min to walk between stages) ==");
if (clashes.Count == 0)
{
    Console.WriteLine("  none, enjoy the festival");
}

foreach (var clash in clashes)
{
    Console.WriteLine($"  {clash}");
}

static TimeOnly? ReadTimeArgument(string[] args)
{
    var index = Array.IndexOf(args, "--at");
    if (index < 0 || index + 1 >= args.Length)
    {
        return null;
    }

    return TimeOnly.TryParseExact(args[index + 1], "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out var time)
        ? time
        : null;
}

static int? ReadWalkArgument(string[] args)
{
    var index = Array.IndexOf(args, "--walk");
    if (index < 0 || index + 1 >= args.Length)
    {
        return null;
    }

    return int.TryParse(args[index + 1], CultureInfo.InvariantCulture, out var minutes) ? minutes : null;
}
