---
name: dotnet-course-planning
description: "Use when planning or revising this instructor's university .NET course, including session schedules, coding assessments, and pair projects."
---

# University .NET Course Planning

## Instructor and course context

- The user teaches computer science at a university.
- The course is an 8-session introduction to .NET, with 3 hours of contact time per session (24 hours total).
- Cover .NET history, tool installation, a Hello World program, unit tests, LINQ, IoC/dependency injection and workers, ASP.NET Core, EF Core, AOT, and Aspire.
- Include exactly two coding tests. Each test is one hour and asks students to write code.
- Students work on a project in pairs during the trimester and present it at the end.
- The instructor already has a LINQ dojo where students rewrite `Select` and `Where` to discover the `yield` keyword. Use this dojo as the central LINQ activity rather than proposing a substitute exercise.
- Students are in their final year and already know how to program. Do not spend course time teaching general programming fundamentals or OOP; focus on .NET-specific tools, libraries, runtime behavior, and idiomatic practices.

## Planning guidance

1. Produce a session-by-session plan with timings that add up to exactly 180 minutes per session.
2. Include clear learning goals, hands-on exercises, and preparation or project milestones.
3. Reserve exactly 60 minutes for each of the two coding tests; describe the programming task and the skills it assesses.
4. Introduce the pair project early enough for students to develop it over the trimester. Include progress checkpoints and reserve time for final presentations.
5. Distribute the required topics coherently, building from .NET tooling and runtime concepts toward a deployable application. Treat students as experienced programmers: skip introductory programming and OOP instruction, and use that time for .NET-specific project structure, CLI/build tooling, testing practices, runtime behavior, architecture, and integration. Connect tests, data access, web APIs, background work, AOT, and Aspire to practical exercises or the project where appropriate. For LINQ, center the lesson on the instructor's existing `Select`/`Where` dojo and the discovery of iterator behavior with `yield`.
6. Treat each scheduled session as 3 hours including its test or presentations. If the course has additional independent project time, distinguish it from contact hours.
7. Avoid assuming a particular SDK version, IDE, operating system, or assessment rubric. Recommend a currently supported .NET SDK and ask only when a missing constraint materially changes the plan.
