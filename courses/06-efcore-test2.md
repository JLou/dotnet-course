# Session 6 — EF Core and coding test 2

**Duration:** 180 minutes  
**Assessment:** One-hour individual coding test.

## Learning outcomes

Students should be able to:

- Explain how EF Core maps a model to a relational database and what `DbContext` represents.
- Configure a context and provider, create/update a schema using migrations, and perform basic CRUD operations.
- Use asynchronous query APIs and understand when a LINQ query executes in the database.
- Recognize tracking, query translation, and common performance pitfalls at an introductory level.
- Implement a small data-backed API feature in a prepared codebase under test conditions.

## Session outline

| Time | Activity |
|---:|---|
| 25 min | Relational data and ORM concepts |
| 35 min | Entities, `DbContext`, and migrations |
| 10 min | Break |
| 35 min | CRUD and LINQ queries with EF Core |
| 60 min | **Coding Test 2** |
| 15 min | Debrief and review |
| **180 min** | **Total** |

## Teaching notes

### Relational data and ORM concepts — 25 min

- Start from the relational schema the API needs: tables, keys, constraints, and relationships.
- Explain object-relational mapping as a mapping between the domain/object model and relational storage, not as a replacement for understanding SQL.
- Introduce `DbContext` as a unit-of-work/change-tracking boundary; it is not intended to be shared concurrently across requests or threads.
- Discuss provider choice as an environmental/architectural decision; use one prepared provider for the workshop.
- Identify what EF Core can generate versus what still requires deliberate schema and query design.

### Model, context, and migrations — 35 min

- Add an entity, a `DbSet<T>`, and context/provider configuration.
- Explain conventions and when explicit configuration/attributes are useful.
- Demonstrate a migration: create, inspect, apply, and explain that migrations are versioned schema changes.
- Distinguish creating a migration from applying it to a database.
- Explain connection strings and configuration sources at a high level; do not put production secrets in source control.
- Keep the provider and database available to every student; SQLite is often convenient for a local teaching exercise if it fits the course needs.

### CRUD, LINQ, and query behavior — 35 min

- Implement asynchronous create and read operations with `SaveChangesAsync` and async query methods.
- Show when query composition happens and when it executes (for example, `ToListAsync`, `FirstOrDefaultAsync`).
- Contrast `IQueryable<T>` provider translation with in-memory `IEnumerable<T>` processing; avoid calling `ToList` too early and pulling an entire table into memory.
- Introduce tracking versus no-tracking reads and when each is suitable.
- Discuss relationship loading and N+1 queries conceptually; inspect generated SQL if time/tooling allows.
- Emphasize cancellation tokens on request-bound asynchronous database work.

### Coding Test 2 — 60 min

**Prompt shape:** Provide a prepared ASP.NET Core/EF Core project with the entity, context/provider configuration, database initialization or migration, and test harness already set up. Ask students to complete one small feature such as list and create, including relevant validation/status behavior.

**Assess:** Correct EF Core query and save usage, suitable asynchronous APIs, integration with the endpoint/application layer, and tests or provided checks for expected results.

**Boundaries:** Do not require environment installation, database-server setup, or migration design from scratch within the hour. Provide explicit expected behavior and a known-good local test path. The assessment is a code-writing test, not a setup challenge.

### Debrief — 15 min

- Explain the most common mistakes: missing `SaveChangesAsync`, client-side materialization too early, wrong handling of missing records, and tests sharing state.
- Discuss how to keep integration tests deterministic and isolated.
- Ask each pair to identify one persistence design decision in its project.

## Preparation and follow-up

- Run the assessment starter from a clean checkout on the exact lab environment.
- Make database creation predictable and provide a resettable test database strategy.
- Pairs should integrate a basic persistent data path into their project.
