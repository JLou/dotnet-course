# Session 1 — .NET basics, history, and tooling

**Duration:** 180 minutes  
**Audience assumption:** Students already know how to program. Treat Hello World as an environment and workflow check, not as a programming lesson.

## Learning outcomes

By the end of the session, students should be able to:

- Describe the major shifts in .NET's history and distinguish the modern .NET platform from the older .NET Framework.
- Install or verify a supported .NET SDK and use the `dotnet` CLI.
- Create, build, and run a project, and identify the key files involved.
- Explain at a high level what the SDK, compiler, runtime, and NuGet contribute.

## Session outline

| Time | Activity |
|---:|---|
| 15 min | Course overview and learning goals |
| 20 min | History and ecosystem |
| 35 min | Install tools and verify the environment |
| 10 min | Break |
| 20 min | Hello World environment check |
| 35 min | CLI and project/solution workflow |
| 30 min | Diagnostics, debugging, and project configuration |
| 15 min | Pair-project launch |
| **180 min** | **Total** |

## Teaching notes

### Course framing — 15 min

- Explain the sequence of topics and how the two coding tests relate to the pair project.
- Set expectations: this is about the .NET platform and its ecosystem, not an introduction to programming or OOP.
- Ask students which languages, platforms, and command-line build tools they already know; use these as comparison points throughout the course.

### History and ecosystem — 20 min

Keep the history selective and connect each milestone to a technical reason:

- .NET Framework as the original Windows-focused platform.
- Mono and other efforts that helped bring .NET to additional platforms.
- .NET Core as the cross-platform, open-source redesign.
- The unified modern .NET releases and the continuing role of C# and the runtime.
- Distinguish the runtime (execution and services such as garbage collection), SDK (build and developer tools), language/compiler, libraries, and application frameworks.
- Place ASP.NET Core, EF Core, and Aspire in the ecosystem map; they are related projects/frameworks, not alternate names for the runtime.

Avoid spending the full history segment on release trivia. The goal is to give students a useful mental model and vocabulary.

### Tool installation — 35 min

- Install or verify a currently supported .NET SDK; choose IDE/editor instructions that fit the lab environment.
- Show `dotnet --info` and `dotnet --list-sdks`. Explain that the SDK version and the runtime used by an application are related but not identical concepts.
- Have students run `dotnet --version` and create a scratch console app using `dotnet new console`.
- Run `dotnet restore`, `dotnet build`, and `dotnet run`; show that `dotnet run` will build when needed.
- Discuss common environment problems: wrong SDK installed, IDE using a different SDK, PATH not refreshed, network/package restore issues, and differing shell syntax.
- If installation is centrally managed, prepare a preflight checklist and a known-good lab machine/image.

### Hello World — 20 min

- Create and run the generated console app. Have students inspect the entry point and change the output.
- Point out that template details vary by SDK/template version; avoid treating one exact generated layout as universal.
- Ask students to identify which command created the files, which command compiled them, and what output they observed.

### CLI and project workflow — 35 min

Demonstrate a small, repeatable command sequence:

```text
dotnet new console -n CourseDemo
dotnet sln CourseDemo.sln add CourseDemo/CourseDemo.csproj
dotnet build CourseDemo.sln
dotnet run --project CourseDemo/CourseDemo.csproj
dotnet test CourseDemo.sln
```

Adjust solution-file creation syntax to the installed SDK's supported templates/CLI conventions. Explain:

- A `.csproj` is an MSBuild project file: it describes the target framework, package/project references, and build properties.
- A solution groups projects for tooling; it is useful but not required for every `dotnet` command.
- Project references are for code shared between projects in the same solution; package references usually bring in NuGet dependencies.
- Restore resolves dependencies; build compiles; run launches; test builds and executes test projects.
- Generated `bin` and `obj` are build outputs/intermediates, not source-of-truth files.
- `global.json` can pin SDK selection for a repository; discuss it as a team reproducibility tool, not a mandatory first-session configuration.

### Diagnostics, debugging, and configuration — 30 min

- Introduce compiler errors versus warnings and show how to navigate from a CLI diagnostic to a source location.
- Demonstrate a breakpoint, stepping, locals/watch, and launch/run configuration in the chosen IDE.
- Inspect the project file: target framework, nullable setting, implicit usings, package references, and simple build properties.
- Show where NuGet package references are declared and explain that restoring a package does not mean copying source into the project.
- Emphasize that the project file is executable build configuration and changes to it should be reviewed like source changes.

### Pair-project launch — 15 min

- Present the trimester pair project and ask pairs to brainstorm a problem with a clear user and a demonstrable outcome.
- Request an initial one-sentence problem statement; formal scope and requirements are refined in Session 2.
- Mention that the final session allows 15 pairs at ten minutes each; adapt the format if enrolment is larger.

## Preparation and follow-up

- Test installation instructions on the actual student operating systems and network.
- Prepare a minimal starter repository or command sheet without relying on one specific IDE.
- Before Session 2, pairs should bring one or two project ideas and be ready to state the intended user and core feature.
