<!--
Sync Impact Report
- Version change: N/A (feature plan)
- Constitution check: PASS before and after design; no exceptions required
- Scope conflict resolved: syntactic URL validation is required by the feature spec and constitution; feed fetching/parsing remains deferred per stakeholder goals
- Follow-up: confirm .NET 10 SDK availability when implementation starts
-->

# Implementation Plan: RSS Reader MVP

**Branch**: `001-rss-reader-mvp` | **Date**: 2026-10-23 | **Spec**: [spec.md](spec.md)

**Input**: Feature specification from `/specs/001-rss-reader-mvp/spec.md`, with stakeholder context from `StakeholderDocuments/ProjectGoals.md` and `StakeholderDocuments/TechStack.md`.

## Summary

Build the local single-user subscription-management MVP as an ASP.NET Core Web API and Blazor WebAssembly frontend. The API keeps subscriptions in memory and exposes list/add operations; the UI submits a URL and immediately displays the updated list. Validate that input is a nonblank absolute HTTP(S) URL, but do not fetch or parse feed content. Reject duplicate URLs to avoid duplicate entries. Persistence, removal, refresh, and item display are out of scope.

## Technical Context

**Language/Version**: C# / .NET 10 LTS (planning assumption; the repository has no project files and the SDK is not available on PATH)

**Primary Dependencies**: ASP.NET Core Web API; Blazor WebAssembly; .NET built-in `Uri` validation; no feed parser or database dependency in MVP

**Storage**: Process-local in-memory collection owned by a backend subscription service; data is lost when the API process stops

**Testing**: xUnit unit tests for validation and service behavior; ASP.NET Core integration tests for API behavior; frontend build and manual browser workflow validation

**Target Platform**: Local development on Windows, macOS, or Linux; browser-based Blazor WebAssembly client

**Project Type**: Web application (separate backend API and frontend WASM client)

**Performance Goals**: Local add/list requests complete interactively; no production throughput target

**Constraints**: Single-user local demo; in-memory state; explicit frontend API base URL, backend CORS origins, and local ports; no network request to the feed URL

**Scale/Scope**: One subscription-management screen; add and list operations; no account system or feed content workflow

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

- Security-first input handling: PASS. Validate absolute HTTP(S) URLs as input only; do not issue outbound requests in this phase.
- Maintainability by design: PASS. One focused subscription service and small API/UI surface; avoid persistence and feed abstractions.
- Test-first and verification-driven delivery: PASS. Define validation/service/API tests and an end-to-end add/list check in `quickstart.md`.
- Quality gates for feed and UI work: PASS. Specify URL validation, visible errors, API URL/CORS/port checks, and route-conflict cleanup before UI feature work.
- Incremental MVP and simplicity: PASS. Fetching, parsing, persistence, background work, and removal remain deferred.

## Project Structure

### Documentation (this feature)

```text
specs/001-rss-reader-mvp/
├── plan.md
├── research.md
├── data-model.md
├── quickstart.md
├── contracts/
│   └── subscriptions-api.md
└── tasks.md
```

### Source Code (repository root)

```text
backend/
├── RSSFeedReader.Api/
│   ├── Contracts/
│   ├── Models/
│   ├── Services/
│   └── Program.cs
└── RSSFeedReader.Api.Tests/
frontend/
├── RSSFeedReader.UI/
│   ├── Layout/
│   ├── Pages/
│   └── wwwroot/
└── RSSFeedReader.UI.Tests/
```

**Structure Decision**: Use the stakeholder-specified `backend/RSSFeedReader.Api` ASP.NET Core API and `frontend/RSSFeedReader.UI` Blazor WebAssembly client. Keep service/model code in the API project and tests alongside each application boundary. Remove template demo pages and their navigation links before adding the single root subscription page, as required by `TechStack.md`.

## Complexity Tracking

No constitution violations or added architectural complexity require justification.
| [e.g., Repository pattern] | [specific problem] | [why direct DB access insufficient] |
