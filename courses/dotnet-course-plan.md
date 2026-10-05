# University .NET Course Plan

## Course overview

- **Audience:** Final-year university computer science students who already know how to program
- **Format:** 8 sessions, 3 hours (180 minutes) per session; 24 contact hours total
- **Pair project:** Students develop a project in pairs throughout the trimester and present it in Session 8.
- **Assessments:** Two individual, one-hour coding tests. Each test is included in its session's 180 minutes.
- **LINQ dojo:** The instructor has an existing dojo in which students rewrite `Select` and `Where` to discover the `yield` keyword. This is the central LINQ activity in Session 3.

## Session schedule

### Session 1 — .NET introduction and setup

**Learning goals:** Understand the history and ecosystem of .NET, set up the development environment, and become comfortable with the .NET CLI and project/build workflow. The Hello World exercise is a quick environment check, not an introduction to programming.

| Time | Activity |
|---:|---|
| 15 min | Course overview and learning goals |
| 20 min | History of .NET and overview of the ecosystem |
| 35 min | Install the .NET SDK and editor; verify the installation |
| 10 min | Break |
| 20 min | Create, run, and briefly inspect a Hello World application |
| 35 min | Explore the CLI, project/solution structure, project references, and build/run/test commands |
| 30 min | Inspect compiler diagnostics, debugging workflow, and package/project configuration |
| 15 min | Introduce the pair project and discuss possible topics |
| **180 min** | **Total** |

### Session 2 — .NET development workflow and unit testing

**Learning goals:** Apply familiar programming skills in the .NET toolchain, create a test project, and use .NET testing conventions and tools effectively. No time is spent teaching OOP fundamentals.

| Time | Activity |
|---:|---|
| 15 min | .NET/C# idioms and setup check; identify differences from languages students already know |
| 30 min | Project and package references, restore/build/test workflow, and useful CLI commands |
| 10 min | Break |
| 45 min | Test-project setup, assertions, test organization, and parameterized tests |
| 30 min | Test seams and test doubles; practice testing code that uses dependencies |
| 30 min | Run tests and diagnose failures with the IDE/CLI; inspect coverage or analyzer feedback |
| 20 min | Pair-project scope and initial requirements |
| **180 min** | **Total** |

### Session 3 — LINQ, iterators, and coding test 1

**Learning goals:** Discover how sequence operators work by implementing `Select` and `Where`, and connect iterator behavior to `yield` and standard LINQ.

| Time | Activity |
|---:|---|
| 15 min | Introduce `IEnumerable<T>` and sequence transformations |
| 55 min | Existing dojo: rewrite `Select` and `Where`, using `yield` to discover iterator behavior |
| 10 min | Break |
| 25 min | Discuss dojo results, deferred execution, chaining, and the standard LINQ operators |
| 60 min | **Coding Test 1:** Write LINQ transformations over a provided dataset and unit tests for key behavior |
| 15 min | Debrief and review |
| **180 min** | **Total** |

### Session 4 — IoC, dependency injection, and workers

**Learning goals:** Understand inversion of control and dependency injection, configure service lifetimes, and implement background work.

| Time | Activity |
|---:|---|
| 30 min | IoC and dependency-injection concepts |
| 35 min | Service registration and lifetimes |
| 10 min | Break |
| 40 min | Workers and `BackgroundService` |
| 45 min | Build a worker that processes queued or periodic work |
| 20 min | Pair-project design checkpoint |
| **180 min** | **Total** |

### Session 5 — ASP.NET Core

**Learning goals:** Understand HTTP API fundamentals and build an ASP.NET Core API using dependency injection and validation.

| Time | Activity |
|---:|---|
| 25 min | HTTP and API fundamentals |
| 35 min | ASP.NET Core application structure and endpoint styles |
| 10 min | Break |
| 50 min | Build a small API |
| 35 min | Add validation, dependency injection, and endpoint tests |
| 25 min | Pair-project checkpoint: demonstrate an API slice |
| **180 min** | **Total** |

### Session 6 — EF Core and coding test 2

**Learning goals:** Model relational data, use EF Core for data access, and implement a small data-backed API feature.

| Time | Activity |
|---:|---|
| 25 min | Relational data and ORM concepts |
| 35 min | Entities, `DbContext`, and migrations |
| 10 min | Break |
| 35 min | CRUD operations and LINQ queries with EF Core |
| 60 min | **Coding Test 2:** Implement a small data-backed API feature in a prepared project and verify its behavior with tests |
| 15 min | Debrief and review |
| **180 min** | **Total** |

### Session 7 — AOT, Aspire, and project integration

**Learning goals:** Understand when ahead-of-time compilation is useful, try an AOT publish, and use Aspire to orchestrate a local distributed application.

| Time | Activity |
|---:|---|
| 25 min | AOT: concepts, trade-offs, and appropriate use cases |
| 30 min | Configure and try an AOT publish |
| 10 min | Break |
| 35 min | Aspire and local distributed-app orchestration |
| 45 min | Integrate a project API and supporting service with Aspire where appropriate |
| 25 min | Project readiness review |
| 10 min | Wrap-up and presentation preparation |
| **180 min** | **Total** |

### Session 8 — Pair-project presentations

**Learning goals:** Demonstrate the project, explain design decisions, and reflect on the course topics applied.

| Time | Activity |
|---:|---|
| 10 min | Setup and presentation guidance |
| 10 min | Break |
| 150 min | Project showcase: 10 minutes per pair (8-minute presentation/demo and 2 minutes for questions), accommodating up to 15 pairs |
| 10 min | Course wrap-up and reflection |
| **180 min** | **Total** |

## Coding tests

1. **Session 3 — LINQ and unit testing (60 minutes):** Given a small dataset and starter project, write code to filter, sort, and summarize records using LINQ, then add unit tests for key behavior. The existing `Select`/`Where` dojo is a learning activity before the test, not a replacement for the test.
2. **Session 6 — ASP.NET Core and EF Core (60 minutes):** Given a prepared project with the model and configuration in place, implement a small data-backed API feature, such as listing and adding records, and verify behavior with tests. Starter code keeps the assessment focused on writing application code rather than setup.

## Pair-project milestones

- **Session 1:** Introduce the project and brainstorm topics.
- **Session 2:** Choose a scope, identify initial requirements, and form pairs.
- **Session 4:** Review the design and identify dependencies and background-work needs.
- **Session 5:** Demonstrate a working API slice.
- **Session 6:** Add persistent data access with EF Core.
- **Session 7:** Integrate relevant topics such as a worker, AOT publishing, or Aspire orchestration; prepare the presentation.
- **Session 8:** Present the finished project.

Students can develop the project between class meetings during the trimester; that independent work is outside the 24 contact hours.

## Detailed instructor notes

- [Session 1 — .NET basics, history, and tooling](./01-basics.md)
- [Session 2 — .NET workflow and unit testing](./02-workflow-testing.md)
- [Session 3 — LINQ, iterators, and coding test 1](./03-linq-iterators.md)
- [Session 4 — Dependency injection, IoC, and workers](./04-di-workers.md)
- [Session 5 — ASP.NET Core APIs](./05-aspnet-core.md)
- [Session 6 — EF Core and coding test 2](./06-efcore-test2.md)
- [Session 7 — AOT, Aspire, and project integration](./07-aot-aspire.md)
- [Session 8 — Pair-project presentations](./08-project-presentations.md)
