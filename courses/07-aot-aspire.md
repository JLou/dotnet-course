# Session 7 — AOT, Aspire, and project integration

**Duration:** 180 minutes

## Learning outcomes

Students should be able to:

- Describe what Native AOT changes about publishing and deployment, and identify compatibility trade-offs.
- Publish a small application with AOT and compare outputs without assuming it is universally faster or smaller.
- Explain Aspire's role in composing and observing local distributed applications.
- Run a small multi-process application through an Aspire AppHost.
- Decide which of these technologies are appropriate for their pair project.

## Session outline

| Time | Activity |
|---:|---|
| 25 min | AOT concepts and trade-offs |
| 30 min | Try an AOT publish |
| 10 min | Break |
| 35 min | Aspire and local orchestration |
| 45 min | Integrate API and supporting service |
| 25 min | Project readiness review |
| 10 min | Presentation preparation and wrap-up |
| **180 min** | **Total** |

## Teaching notes

### AOT — 25 min

- Define ahead-of-time compilation in contrast with the usual JIT-based execution path.
- Explain potential benefits: deployment characteristics, startup behavior, and sometimes reduced runtime requirements or size, depending on application and publish configuration.
- Explain trade-offs: platform-specific native output, additional build requirements, reflection/dynamic-code constraints, trimming analysis, and library compatibility.
- Clarify that AOT is not a universal performance switch; benchmark the relevant workload and inspect the actual publish output.
- Introduce trimming and why reflection-heavy code may require annotations or configuration.

### AOT publish — 30 min

- Use a small prepared application and the supported AOT publishing workflow for the selected SDK and target OS.
- Show the required project settings or template options, then run the publish command for the actual target runtime identifier.
- Inspect the output directory and try running the published artifact independently of the development host where possible.
- Compare publish size/startup only as an observation, noting build mode, platform, and measurement limitations.
- Keep a JIT/non-AOT path available if lab machines lack native compiler prerequisites or if a dependency is incompatible.

### Aspire — 35 min

- Position Aspire as tooling and components for orchestrating and observing distributed applications during development.
- Explain the roles of the AppHost and service projects/resources; distinguish local orchestration from a production deployment platform.
- Demonstrate a small AppHost that starts an API and a supporting dependency, such as a database or cache, using prepared configuration.
- Show service discovery/reference wiring and the dashboard's logs, traces, and resource health where available.
- Discuss that Aspire can improve local developer experience, but is optional and should solve a real multi-service development problem.

### Integration exercise — 45 min

- Start with an API and one supporting service/process.
- Add them to an Aspire AppHost using the installed SDK's supported templates and APIs.
- Run the composed system and inspect startup, health, logs, and service-to-service configuration.
- Have students identify which configuration belongs to local development versus deployment.
- Keep the scope to local orchestration; do not imply Aspire automatically solves production deployment, secrets management, or operational governance.

### Project readiness — 25 min

- Each pair demonstrates its current project and identifies the smallest remaining work to present.
- Review required evidence: a working user-facing flow, persistent data if applicable, tests, and a clear explanation of design choices.
- Ask pairs to justify whether a worker, AOT, or Aspire belongs in their project. These are learning topics, not mandatory project checklist items.

### Presentation preparation — 10 min

- Confirm the presentation order and time limit.
- Require a reliable demo path and a fallback (screenshots, recorded output, or seeded local data) in case live infrastructure fails.

## Preparation and follow-up

- Prepare known-good AOT and Aspire samples and verify prerequisites on lab machines.
- Have a non-AOT fallback and a pre-restored Aspire sample available.
- Pairs should freeze feature scope, test their demo from a clean start, and prepare a concise architecture explanation.
