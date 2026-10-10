# Session 2 — Hands-on testing with TUnit, then collections and LINQ

**Duration:** 180 minutes
**Shape:** Practical. Students write code for most of the session.
**Audience assumption:** Final-year students who already program. Spend the time on .NET tooling,
test-project mechanics and idiomatic LINQ, not on explaining what a unit test is.

## Learning outcomes

Students should be able to:

- Create a test project from the CLI, reference the project under test, and run tests with `dotnet test`.
- Write TUnit tests with assertions, exception assertions, and collection assertions.
- Write parameterized tests with `[Arguments]` and `[MethodDataSource]`.
- Run a subset of tests with `--treenode-filter` and read a failing test report.
- Choose the right .NET collection type and describe what `IEnumerable<T>` actually guarantees.
- Solve common data questions with LINQ operators and recognize deferred execution.

## Session outline

| Time | Activity |
|---:|---|
| 10 min | Testing in .NET: test projects, runners, TUnit |
| 30 min | Guided demo: run the app, create the test project, reference, run, basic and parameterized tests |
| 45 min | Hands-on: students test the Festival Planner library |
| 5 min | Testing debrief |
| 10 min | Break |
| 15 min | Collections in .NET |
| 10 min | LINQ operator tour and first look at deferred execution |
| 40 min | LINQ kata |
| 10 min | Kata debrief and bridge to Session 3 |
| 5 min | Pair project: form pairs and assign the scope statement |
| **180 min** | **Total** |

## Materials

- [`testing/`](../testing) — the **Festival Planner**: a library plus a runnable console demo, and
  its README. It ships **without** a test project on purpose: creating one is part of the lesson.
- LINQ kata: <https://programmingheadache.com/2024/08/04/unlocking-linq-basic-katas-for-c-programmers/>
  (repository `https://github.com/FractalCodeRicardo/katas-linq.git`, cloned by students).

### The Festival Planner in one minute

Chroma Festival publishes its lineup as `stage;artist;start;end` lines. The library has three
features, each aimed at a different testing technique:

| Feature | Type | Best tested with |
|---|---|---|
| Read the lineup file | `LineupParser` | parameterized tests |
| Explore the programme | `Lineup` | collection assertions |
| Find the clashes in a plan | `ClashDetector` | parameterized + exception assertions |

`FestivalSet` is the shared model: one artist, one stage, a start (included) and an end (excluded).

## Teaching notes

### Testing in .NET — 10 min

- Test project = a normal project that references the project under test. The dependency goes one
  way only: tests know about production code, never the reverse.
- Name the main frameworks (xUnit, NUnit, MSTest, TUnit) in one slide, then commit to **TUnit** for
  the course and stop comparing.
- Explain what is specific about TUnit: it is source-generated, tests are `async`, assertions are
  awaited (`await Assert.That(x).IsEqualTo(y)`), and it runs on Microsoft.Testing.Platform instead
  of VSTest. That last point is why [`testing/global.json`](../testing/global.json) sets
  `"test": { "runner": "Microsoft.Testing.Platform" }`; without it the .NET 10 SDK refuses to run
  the tests. Show the error once so students recognize it.
- State the goal of the next 75 minutes: every student ends the block with their own test project
  containing basic and parameterized tests that run from the CLI.

### Guided demo — 30 min

**Start by running the app, not by writing a test.** From `testing/`:

```bash
dotnet run --project src/FestivalPlanner.Cli
dotnet run --project src/FestivalPlanner.Cli -- --at 23:15 --walk 25
```

Read the output together: the programme per stage, the headliners, who is on stage at a given
moment, and the clash list. Point out that the clash report already distinguishes "these two
overlap" from "you cannot walk there in time". Make the point explicitly — **you cannot write a
useful test for behaviour you have never observed.**

Then create the test project live. Students watch first and repeat during the hands-on block.

```bash
dotnet new install TUnit.Templates          # once per machine
dotnet new TUnit -n FestivalPlanner.Tests -o tests/FestivalPlanner.Tests
dotnet add tests/FestivalPlanner.Tests reference src/FestivalPlanner
dotnet sln add tests/FestivalPlanner.Tests
dotnet test
```

Points to make while typing:

- You reference the **library**, not the CLI. The console app is a demo; the behaviour worth
  testing lives in `FestivalPlanner`. This is why the solution is split in two projects.
- The template drops sample files (`BasicTests.cs`, `Calculator.cs`, `DataDrivenTests.cs`,
  `DependencyInjectionTests.cs`, `HooksAndLifecycle.cs`, `Data/`). Delete them, but read
  `DataDrivenTests.cs` aloud first: it is a free cheat sheet for parameterized tests.
- Show the `<ProjectReference>` that `dotnet add reference` wrote in the `.csproj`, and the
  `<PackageReference Include="TUnit" />` that NuGet restores. Open `FestivalPlanner.slnx` too: the
  .NET 10 SDK writes solutions in the new XML format, which is finally readable.
- Show `dotnet test --list-tests`: parameterized cases appear as separate entries with their
  arguments, which is the main reason to prefer them over a loop inside one test.
- Show filtering: `dotnet test --treenode-filter "/*/*/LineupParserTests/*"`. Mention that the
  VSTest-style `--filter-method` / `--filter-class` options do **not** work here, which is a
  concrete consequence of the runner choice.

Then write two tests in front of the class, thinking aloud about arrange/act/assert:

```csharp
namespace FestivalPlanner.Tests;

public class FestivalSetTests
{
    private static readonly DateOnly Day = new(2026, 7, 11);

    [Test]
    public async Task Duration_IsTheTimeOnStage()
    {
        var volta = LineupParser.Parse("Mainstage;Volta;21:15;22:30", Day);

        await Assert.That(volta.Duration).IsEqualTo(TimeSpan.FromMinutes(75));
    }

    [Test]
    [Arguments("21:14", false)]
    [Arguments("21:15", true)]
    [Arguments("22:29", true)]
    [Arguments("22:30", false)]
    public async Task IsPlayingAt_ExcludesTheEndOfTheSet(string moment, bool expected)
    {
        var volta = LineupParser.Parse("Mainstage;Volta;21:15;22:30", Day);
        var at = Day.ToDateTime(TimeOnly.Parse(moment));

        await Assert.That(volta.IsPlayingAt(at)).IsEqualTo(expected);
    }
}
```

The second one is worth dwelling on: four boundary cases in one method, each reported separately,
and the four values are exactly the ones a reviewer would ask about. Use it to make the case that
a parameterized test is a *table of decisions*, not a way to save typing.

Finish by making a test fail on purpose (change `75` to `70`) and reading the failure message
together: expected value, actual value, and the exact case that failed.

### Hands-on — 45 min

Students follow [`testing/README.md`](../testing/README.md), which lists the behaviour to cover
feature by feature. Suggested checkpoints:

- **15 min in:** everyone has a green `dotnet test` with at least one test.
- **30 min in:** everyone has a parameterized test over `LineupParser`, including the midnight cases.
- **Last 15 min:** collection assertions on `Lineup`, exception assertions on `Lineup.Add` and
  `ClashDetector`.

Circulate rather than lecture. Useful prompts:

- "What is the smallest input that would make this test fail if the implementation were wrong?"
- "You wrote six near-identical tests — can `[Arguments]` express that?"
- "Is that test checking behaviour, or is it re-writing the implementation in the assertion?"
- "Your lineup fixture has eight sets and the test only needs two. What is the other six doing?"

**Deliberate traps.** The library is correct enough to look finished and wrong enough to argue
about. Do not reveal these; let students find them and collect the answers in the debrief.

- **The midnight rollover.** `LineupParser` resolves `23:00;00:30` onto two calendar days. A
  student who tests only afternoon sets will never notice; a student who tests the headliners will.
  This is the single most valuable bug in the project.
- **`Mainstage;Volta;23:00;23:00` silently produces a 24-hour set.** The end is not after the
  start, so the parser rolls it to the next day. Nobody wanted that. There is no validation of a
  maximum duration.
- **`NightEndsAt` is 06:00, and the comparison is strict.** A set announced at exactly `06:00` is
  resolved as a *morning* set, fifteen hours before the festival starts. Is 06:00 even the right
  cutoff?
- **Half-open intervals.** `IsPlayingAt` excludes the end instant and `Overlaps` is strict, so
  back-to-back sets do not clash. Defensible — but was it decided, or did it just happen?
- **Walking time is a single global value.** Real festivals do not have equidistant stages.
- **`ClashDetector` compares every pair.** A plan with twenty sets does 190 comparisons. Fine here;
  ask when it would stop being fine.

### Testing debrief — 5 min

- One student shows a parameterized test; name the trade-off between many `[Arguments]` lines and a
  `[MethodDataSource]`.
- Collect two or three traps they found. Make the point explicitly: tests are where the team writes
  down decisions the specification left open. The midnight rollover is the headline example — the
  code *has* a rule, nobody wrote it down, and only a test makes it visible.
- Mention what the course is deliberately deferring: test doubles and seams come back in Session 4
  with dependency injection, where they are natural rather than artificial.

### Collections in .NET — 15 min

- `T[]`, `List<T>`, `Dictionary<TKey, TValue>`, `HashSet<T>`, `Queue<T>`/`Stack<T>`: what each one
  is good at, and the cost of the operation students will reach for most (`Contains`, indexing,
  insertion).
- Interfaces over implementations: `IEnumerable<T>`, `ICollection<T>`, `IList<T>`, `IReadOnlyList<T>`.
  Show `Lineup.Sets` returning `IReadOnlyList<FestivalSet>` as a concrete example of exposing a
  sequence without exposing the mutable list behind it.
- `IEnumerable<T>` only promises "you can walk me once, forwards". It does not promise a count, an
  index, or that walking twice is free or even gives the same result.
- Equality and comparers: `SortedSet<string>(StringComparer.OrdinalIgnoreCase)` as used by
  `Lineup.Stages` — one line that does deduplication, ordering and case-insensitivity at once.

### LINQ tour — 10 min

- LINQ operators are extension methods over `IEnumerable<T>`. Group them by intent rather than
  listing them alphabetically: filter (`Where`), project (`Select`, `SelectMany`), order
  (`OrderBy`, `ThenBy`), group (`GroupBy`, `ToLookup`), aggregate (`Count`, `Sum`, `Average`,
  `Aggregate`), element (`First`, `FirstOrDefault`, `Single`), materialize (`ToList`,
  `ToDictionary`).
- Good hook: most of `Lineup` is hand-written loops. Rewrite `Stages` or `Headliners` with LINQ on
  the board and compare. The loop version was not wrong — it was just longer.
- Insist on the `*OrDefault` pairs and on `Single` vs `First`: choosing the wrong one is the most
  common bug in the kata.
- Give deferred execution as an observation to verify, not a lecture: a query built with `Where`
  runs nothing until it is enumerated. Session 3 explains *why* when they implement it themselves.
- Keep query syntax to a single slide; the course uses method syntax.

### LINQ kata — 40 min

```bash
git clone https://github.com/FractalCodeRicardo/katas-linq.git
cd katas-linq
dotnet build
dotnet test
```

- The repository provides an `Animal` record, a fixed list, and a `Katas` class of unimplemented
  methods with tests already written (15 of them, all red at first). Students implement the methods
  until the suite is green.
- This inverts the morning's exercise: tests are given, production code is missing. Say that out
  loud — it is the same red/green loop seen from the other side.
- Two environment caveats, both worth 30 seconds of warning:
  - The kata targets `net8.0` and uses MSTest on VSTest. If a machine only has the .NET 10 runtime,
    change `<TargetFramework>` to `net10.0` in `linq-katas.csproj`.
  - It must be cloned **outside** the course repository. Cloning it under `testing/` would put it
    below [`testing/global.json`](../testing/global.json), whose MTP runner setting makes
    `dotnet test` reject this VSTest-based project.
- Order matters less than finishing; tell students it is fine to stop around
  `GetCountOfEachAnimalType` and come back later.

### Kata debrief and bridge — 10 min

- Compare solutions for `GetDuplicatedAnimalName` (`GroupBy` + `Where`) and
  `GetCountOfEachAnimalType` (`GroupBy` + `ToDictionary`, or `CountBy`).
- Show one case where a readable chain beats a clever one-liner.
- Bridge to Session 3: "You used `Where` and `Select` all afternoon. Next session you write them."

### Pair project — 5 min

- Form pairs and give the project brief.
- Homework before Session 3 or 4: a one-page scope statement — user/problem, core capability, and
  the smallest end-to-end slice they could demo in Session 8.

## Preparation and follow-up

- Verify on the classroom machines: `dotnet run --project src/FestivalPlanner.Cli`, then
  `dotnet new install TUnit.Templates` and the full command sequence above, including
  `dotnet test` at solution level.
- Have the NuGet packages pre-cached or a local feed ready if the room's network is slow; the TUnit
  template restore is the main failure point.
- Clone and build the kata repository beforehand, outside the course repository, and note whether
  the classroom SDK needs the `net10.0` target-framework bump.
- Keep a reference test suite for yourself, but do not publish it: the point is that students decide
  what the ambiguous cases should do.
- Collect the pair scope statements before Session 4.
