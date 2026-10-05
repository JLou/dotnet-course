# Session 3 — LINQ, iterators, and coding test 1

**Duration:** 180 minutes  
**Central activity:** The instructor's existing dojo, where students rewrite `Select` and `Where` and discover `yield`.

## Learning outcomes

Students should be able to:

- Explain the role of `IEnumerable<T>` in representing a sequence.
- Implement the essential behavior of `Select` and `Where` with iterator methods.
- Explain how `yield return` turns a method into an iterator and how iteration advances.
- Predict when deferred LINQ queries execute and what happens when a query is enumerated more than once.
- Apply LINQ to a small data transformation and test its observable behavior.

## Session outline

| Time | Activity |
|---:|---|
| 15 min | `IEnumerable<T>` and sequence transformations |
| 55 min | Existing `Select`/`Where` dojo with `yield` |
| 10 min | Break |
| 25 min | Debrief: deferred execution, chaining, standard operators |
| 60 min | **Coding Test 1** |
| 15 min | Debrief and review |
| **180 min** | **Total** |

## Teaching notes

### Sequence model — 15 min

- Begin with a sequence pipeline students can recognize: source, filter, projection, terminal operation.
- Distinguish `IEnumerable<T>` (a way to enumerate values) from a materialized collection such as `List<T>`.
- Review the `Func<T, bool>` predicate and `Func<TSource, TResult>` selector signatures needed for `Where` and `Select`.
- Ask students to predict what runs when a query is declared versus when it is enumerated.

### Existing dojo — 55 min

Use the instructor's existing exercise rather than substituting another LINQ exercise. Let the rewrite create the need for the language feature:

- Students implement `Select` and `Where` over `IEnumerable<T>`; their versions may be extension methods or ordinary static methods depending on the dojo setup.
- First make a straightforward implementation using an explicit result collection. Have students discuss allocation and eager evaluation.
- Then guide them toward returning values one at a time using `yield return`.
- Trace control flow: the iterator body begins when enumeration starts, pauses at each `yield return`, and resumes on the next request for an item.
- Observe behavior with `foreach`, `ToList()`, and a side effect in the source or selector. Use side effects only as a temporary teaching probe.
- Discuss why `Where` and `Select` compose as pipelines and why neither needs to store the entire result.
- If the dojo includes `yield break`, connect it to terminating an iterator early.

Keep the discovery student-led. Avoid presenting `yield` implementation details before students have encountered the problem the feature solves.

### Debrief and LINQ behavior — 25 min

- Name the language feature discovered in the dojo: iterator blocks and compiler-generated state machines. Keep the state-machine explanation conceptual unless students ask for implementation detail.
- Explain deferred execution: many LINQ-to-Objects operators describe work that runs when enumerated; terminal operations such as `Count`, `First`, and `ToList` cause enumeration.
- Show that some operations are streaming (`Where`, `Select`) while others may need more data or materialization (`OrderBy`, `GroupBy`, `ToList`).
- Discuss repeated enumeration: it may repeat expensive work or observe changed source data; materialize deliberately when a stable snapshot is needed.
- Clarify that `IQueryable<T>` and EF Core queries are not simply the same as in-memory `IEnumerable<T>` queries: query providers translate expression trees and have translation constraints. This preview connects to Session 6.

### Coding Test 1 — 60 min

**Prompt shape:** Provide a small domain model and in-memory dataset. Ask students to implement a few named transformations, such as filtering eligible records, projecting a result, ordering or grouping, and computing a summary. Require focused tests for normal and boundary cases.

**Assess:** Correct use of LINQ, readable composition, appropriate terminal operations, and tests that validate behavior.

**Keep the task bounded:** Provide a working starter project and clarify expected output. Do not require students to recreate the dojo operators during the timed test. Test the course outcomes without turning the assessment into a speed exercise in environment setup.

### Debrief — 15 min

- Review the most common errors and at least one alternate valid query formulation.
- Revisit eager versus deferred evaluation and ask students to identify where the query executes.
- Explain how the test will be reviewed and connect any repeated patterns to the next session's service boundaries.

## Preparation and follow-up

- Run the dojo against the SDK/compiler environment students will use.
- Have starter and reference implementations available, but do not expose the reference before the discovery.
- Prepare a small test dataset and transparent rubric for the coding test.
- Ask pairs to identify where sequence transformation might be useful in their project.
