# Research: RSS Reader MVP

## Decisions

### Application architecture

- **Decision**: Use a separate ASP.NET Core Web API backend and Blazor WebAssembly frontend.
- **Rationale**: This is the architecture explicitly selected in `StakeholderDocuments/TechStack.md` and supports the local MVP as well as later phases without requiring a rewrite.
- **Alternatives considered**: A single Blazor application could reduce initial wiring, but it would depart from the agreed stack and would not exercise the planned API boundary.

### Runtime version

- **Decision**: Plan against .NET 10 LTS.
- **Rationale**: It is an LTS release available as of the planning date and is a reasonable baseline for a new project. No project files or `global.json` exist yet, so this is an implementation-time assumption to confirm.
- **Alternatives considered**: .NET 8 LTS and .NET 9 STS; choosing a supported LTS baseline avoids starting a new project on a short-term-support release.

### Subscription storage

- **Decision**: Keep subscriptions in a process-local in-memory service in the backend.
- **Rationale**: The project goals explicitly limit the MVP to in-memory state and defer persistence.
- **Alternatives considered**: Browser-only state would avoid API storage, but the agreed stack assigns subscription management to the backend API.

### URL validation and duplicate handling

- **Decision**: Require nonblank absolute HTTP or HTTPS URLs and reject duplicates; do not contact the URL or verify it serves RSS/Atom.
- **Rationale**: The feature specification requires malformed-input rejection and disallows broken or duplicate entries; the constitution requires untrusted input validation. Syntax validation reconciles these requirements with the stakeholder goal of avoiding feed validation, fetching, and parsing.
- **Alternatives considered**: Accepting any string follows one sentence in ProjectGoals but conflicts with the feature acceptance criteria and security principle. Checking reachability or parsing feed content would exceed MVP scope.

### API boundary

- **Decision**: Provide `GET /api/subscriptions` and `POST /api/subscriptions`.
- **Rationale**: These are the smallest operations needed for the frontend to display current state and add an entry through the API.
- **Alternatives considered**: Combining list and add into one endpoint would complicate normal HTTP semantics without reducing meaningful project scope.

### UI validation and errors

- **Decision**: Show the current list, a URL input, an add action, and an inline success/error result; load the list from the API on page initialization.
- **Rationale**: This directly covers the independent user workflows and ensures state is owned by the backend during the running session.
- **Alternatives considered**: Optimistic-only client state could become inconsistent with API validation; reloading the list after success keeps the UI behavior straightforward.

## Validation Notes

- `setup-plan.ps1 -Json` identified feature branch `001-rss-reader-mvp` and copied the plan template.
- The workspace currently contains no application project files, and `dotnet --version` could not run because `dotnet` is not available on PATH. Install/enable the SDK before implementing or executing .NET validation.
- No `.specify/extensions.yml` exists, so no planning hooks were registered.