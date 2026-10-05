# Session 4 — Dependency injection, IoC, and workers

**Duration:** 180 minutes

## Learning outcomes

Students should be able to:

- Explain inversion of control and how the .NET dependency-injection container constructs application services.
- Register and resolve services using appropriate lifetimes.
- Identify common lifetime mismatch problems.
- Implement a hosted background worker with cancellation-aware asynchronous work.
- Connect a worker to an application service without coupling it to transport or persistence details.

## Session outline

| Time | Activity |
|---:|---|
| 30 min | IoC and dependency-injection concepts |
| 35 min | Registration and service lifetimes |
| 10 min | Break |
| 40 min | `BackgroundService` and hosted workers |
| 45 min | Build a queued or periodic worker |
| 20 min | Pair-project design checkpoint |
| **180 min** | **Total** |

## Teaching notes

### IoC and dependency injection — 30 min

- Start with code that directly constructs its dependencies. Ask what becomes difficult to replace or configure.
- Show constructor injection: a consumer declares what it needs; the composition root provides implementations.
- Define inversion of control in practical terms: application code no longer owns every construction decision.
- Trace the composition root in a .NET host: services are registered during startup and resolved by the host/container.
- Prefer constructor injection for required dependencies. Avoid service locator patterns that resolve arbitrary dependencies inside business logic.
- Discuss that DI improves replaceability and testability but is not a goal by itself; avoid gratuitous abstraction.

### Registrations and lifetimes — 35 min

Explain the built-in container's common lifetimes:

- **Transient:** a new instance is created each time it is requested.
- **Scoped:** one instance per scope; in web applications, commonly one scope per request.
- **Singleton:** one instance for the lifetime of the host.

Then cover:

- Registration patterns for interfaces and implementations and how registrations are wired to the host.
- Why singleton services must be safe for concurrent use if used concurrently.
- Captive dependency/lifetime mismatch: a longer-lived object should not retain a shorter-lived scoped object.
- A hosted service is generally a singleton. If it needs a scoped dependency such as a `DbContext`, create a scope per unit of work using `IServiceScopeFactory`, then dispose it.
- Do not teach lifetimes as arbitrary performance switches; choose based on state and ownership semantics.

### Workers — 40 min

- Explain the generic host and hosted-service lifecycle: start, execute, cancellation, and stop.
- Introduce `BackgroundService` and the `ExecuteAsync` loop.
- Pass the supplied cancellation token to delays and asynchronous operations. Avoid uninterruptible infinite loops and fire-and-forget work.
- Discuss graceful shutdown and what happens if a unit of work is interrupted.
- Compare a simple periodic worker with a queue consumer. Emphasize that this course exercise is local background processing, not a durable distributed message broker.
- Discuss idempotence and retry concerns conceptually; a retry can repeat work if completion was not recorded.

### Practical worker — 45 min

Have students implement a worker that periodically polls or processes items from a small queue:

1. Define a small work abstraction and register it with the host.
2. Implement cancellation-aware processing.
3. Log start, completion, and failures with structured logging.
4. Add a test for the processing service separately from the long-running host loop.
5. Run the host and observe graceful shutdown.

Avoid tests that hang waiting for a real timer; inject a controllable boundary or test one unit of work directly.

### Project checkpoint — 20 min

- Each pair presents its current component sketch and names the composition root.
- Ask whether a background worker is genuinely useful (scheduled cleanup, import, notification, etc.) or unnecessary scope.
- Review dependencies crossing API, application, and persistence boundaries. Require a reason for each abstraction.

## Preparation and follow-up

- Prepare a small Generic Host sample with one registered service and one `BackgroundService`.
- Include one deliberate lifetime mismatch for discussion, but do not leave it in the finished sample.
- Pairs should record whether their project needs background work and what its trigger and failure behavior are.
