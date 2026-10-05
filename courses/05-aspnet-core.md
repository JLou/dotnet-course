# Session 5 — ASP.NET Core APIs

**Duration:** 180 minutes

## Learning outcomes

Students should be able to:

- Explain the HTTP request/response model used by a small web API.
- Locate endpoint mapping and middleware configuration in an ASP.NET Core application.
- Implement endpoints, bind input, return appropriate status codes, and use dependency injection.
- Validate incoming data and write useful endpoint-level tests.
- Demonstrate a working API slice for the pair project.

## Session outline

| Time | Activity |
|---:|---|
| 25 min | HTTP and API fundamentals |
| 35 min | ASP.NET Core structure and endpoint styles |
| 10 min | Break |
| 50 min | Build a small API |
| 35 min | Validation, DI, and endpoint tests |
| 25 min | Pair-project API checkpoint |
| **180 min** | **Total** |

## Teaching notes

### HTTP and API fundamentals — 25 min

- Review methods, paths, headers, request bodies, status codes, and content types as an API contract.
- Use a concrete resource example and map operations to GET/POST/PUT/PATCH/DELETE only where appropriate.
- Distinguish transport concerns (HTTP status and serialization) from application rules.
- Discuss error responses as part of the contract. Avoid returning success-shaped responses when input or processing fails.
- Demonstrate requests with `curl`, an IDE HTTP client, or another lab-standard tool.

### Application structure and endpoints — 35 min

- Create or inspect an ASP.NET Core Web API project.
- Trace startup and the request pipeline: service registration, middleware, endpoint mapping, and host run.
- Explain middleware as components that can inspect/modify a request/response and either continue or short-circuit.
- Compare Minimal APIs and controller-based APIs briefly. Use one approach for the hands-on exercise; explain when a team may prefer the other.
- Show route parameters, query parameters, JSON body binding, and DI-provided services.
- Explain that endpoint styles are alternatives within ASP.NET Core, not separate web frameworks.

### Build a small API — 50 min

Students implement a small in-memory API feature:

1. Define a resource contract and routes.
2. Implement list and create operations plus one lookup or update operation if time permits.
3. Return meaningful status codes, including not found and created outcomes.
4. Separate the HTTP endpoint from the operation's core logic where useful.
5. Exercise the endpoints using an HTTP client and inspect both request and response.

Keep persistence in memory for this session so EF Core can be introduced coherently in Session 6.

### Validation, DI, and tests — 35 min

- Validate untrusted request data at the boundary and return a clear client error for invalid input.
- Explain that syntactic validation and domain rules are related but not identical.
- Inject the application service rather than constructing it in an endpoint.
- Add endpoint tests using the framework's supported test hosting approach if preconfigured; otherwise use focused unit tests for core logic plus manual HTTP verification.
- Explain the distinction between a unit test of application logic and an integration test that exercises routing, middleware, serialization, and DI.
- Mention authentication/authorization and OpenAPI as production topics without letting them displace the planned fundamentals.

### Pair-project checkpoint — 25 min

- Each pair demonstrates one API route and states its request/response contract.
- Give feedback on route naming, status codes, validation, and separation of concerns.
- Identify what data must persist; this becomes the Session 6 EF Core task.

## Preparation and follow-up

- Prepare an API starter sample and a short request collection or `.http` file.
- Keep the example domain small and compatible with the later EF Core exercise.
- Ask pairs to write a few endpoint examples and expected outcomes before Session 6.
