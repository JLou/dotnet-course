# Session 2 — .NET workflow and unit testing

**Duration:** 180 minutes  
**Audience assumption:** Students already understand programming, object-oriented design, and basic testing ideas. Spend time on how these practices work in .NET, not on teaching OOP from scratch.

## Learning outcomes

Students should be able to:

- Navigate a small multi-project .NET repository and use restore, build, and test workflows.
- Create or understand a test project and run selected tests from the CLI or IDE.
- Write focused unit tests, including parameterized tests, using the course's chosen test framework.
- Recognize when dependencies make code difficult to test and use test doubles appropriately.
- Choose a feasible pair-project scope and define an initial vertical slice.

## Session outline

| Time | Activity |
|---:|---|
| 15 min | .NET/C# idioms and setup check |
| 30 min | Project/package references and CLI workflow |
| 10 min | Break |
| 45 min | Test project, assertions, organization, parameterization |
| 30 min | Test seams and test doubles |
| 30 min | Diagnose tests; coverage/analyzer feedback |
| 20 min | Pair-project scope and requirements |
| **180 min** | **Total** |

## Teaching notes

### Idioms and setup check — 15 min

- Check that all students can build a starter project and run its tests.
- Invite brief comparisons with tools they know, such as Maven/Gradle, npm, pytest, or another compiler/test runner.
- Explain that the goal is to learn .NET conventions rather than re-teach classes, interfaces, or basic algorithms.
- Identify differences students are likely to encounter: project files, NuGet, target frameworks, nullable reference types, and the `dotnet` command family.

### Project and package workflow — 30 min

- Start from a repository containing an application/library project and a test project.
- Trace the dependency direction: test project references production project; application code should not depend on its tests.
- Run restore, build, and test at solution and project scope. Use `dotnet test --list-tests` if supported by the chosen test setup.
- Explain package references in the project file and the role of NuGet restore. Avoid implying that package versions are automatically safe or reproducible without source control and review.
- Show how a target framework affects available APIs and runtime targets.
- Briefly discuss nullable reference analysis as compiler feedback, not a substitute for validating external input.

### Test-project setup and test design — 45 min

- Use the course's chosen test framework consistently; avoid comparing several frameworks in depth.
- Explain the test project convention, test discovery, assertions, and the red/green/refactor loop.
- Write tests around observable behavior and meaningful boundary cases, not private implementation details.
- Demonstrate parameterized tests for multiple input/output cases.
- Contrast deterministic unit tests with tests that depend on network, time, filesystem, or database state.
- Have students implement tests for a small provided library or a project-domain rule.

### Test seams and doubles — 30 min

- Present a service that depends on an external boundary such as a clock, file store, or remote client.
- Explain a test seam: a boundary through which a test can control or observe behavior.
- Distinguish a fake (working lightweight implementation), stub (predefined responses), and mock/interaction assertion (verify a call); focus on the testing purpose rather than terminology disputes.
- Use interfaces or other existing abstractions when appropriate, without turning every class into an interface solely to satisfy a mocking framework.
- Discuss the trade-off between isolated unit tests and a smaller number of integration tests.

### Diagnostics and feedback — 30 min

- Give students a failing test suite and have them diagnose a failure using both IDE and `dotnet test` output.
- Show how to run one test or one test class using the test runner's filtering syntax; syntax can differ by test framework/SDK.
- If tooling is available, show coverage output and one analyzer diagnostic. Explain what each signal can and cannot tell you.
- Do not equate high line coverage with strong tests.

### Pair-project scope — 20 min

- Each pair states the user/problem, core capability, and smallest end-to-end slice.
- Identify a likely HTTP/API boundary and the persistent data they may need.
- Encourage a scope small enough to be demonstrable in Session 8; optional technologies should support the project's purpose rather than be added as checklist features.

## Preparation and follow-up

- Prepare a tiny solution with a library, test project, one failing test, and a dependency boundary.
- Provide a one-page command reference for the chosen test runner.
- Pairs should submit a short scope statement and a first list of domain data/features before Session 3 or 4.
