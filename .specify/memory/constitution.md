<!--
Sync Impact Report
- Version change: 0.0.0 → 1.0.0
- Modified principles: N/A (no prior project-specific constitution existed; template content was replaced with project-specific governance)
- Added sections: Security-First Input Handling, Maintainability by Design, Test-First and Verification-Driven Delivery, Quality Gates for Feed and UI Work, Incremental MVP and Simplicity, Project-Specific Constraints, Development Workflow
- Removed sections: Generic placeholder principles and example scaffolding
- Follow-up TODOs: none
-->

# RSS Feed Reader Constitution

## Core Principles

### I. Security-First Input Handling
All external feed URLs, network requests, configuration values, and user-provided text MUST be treated as untrusted input. Validation must occur before a URL is accepted or a request is sent, and connection settings must be explicit and environment-safe. No secrets, tokens, or connection strings may be committed to source control. Feed content must be rendered safely and avoid untrusted HTML or script execution.

### II. Maintainability by Design
The project MUST remain easy to understand, review, and extend. Code must use clear naming, narrow responsibilities, and the simplest implementation that satisfies the current requirement. New features must not add unnecessary abstractions, duplicate logic, or broad scope without justification. The architecture must stay aligned to the MVP definition and only expand when the current phase is proven stable.

### III. Test-First and Verification-Driven Delivery
Before implementing a behavior, bug fix, or integration change, the relevant acceptance criteria MUST be captured in a test or explicit verification step. The team MUST validate both the build and the affected user workflow before considering the work complete. A feature is not finished until it has been exercised in the scenario it changes.

### IV. Quality Gates for Feed and UI Work
All feed-related changes MUST be reviewed for input validation, parsing robustness, and user-visible failure states. Frontend and backend changes MUST verify port configuration, CORS policy, and routing integrity before they are treated as complete. Ambiguous routes, broken API configuration, or missing validation are non-negotiable defects for this project.

### V. Incremental MVP and Simplicity
The project MUST prioritize a working minimal MVP over broad feature scope. The first successful milestone is subscription management: users can add a feed and see it in the list. Feed fetching, persistence, background processing, and advanced display features remain deferred until the MVP is proven stable and the project scope is intentionally expanded.

## Project-Specific Constraints
This project uses an ASP.NET Core Web API backend and a Blazor WebAssembly frontend. The backend and frontend MUST remain connected through explicit configuration values and an approved CORS policy. Feed subscriptions must be treated as externally sourced data and handled with clear validation and graceful failure states.

The MVP remains intentionally constrained to a single-user, local-development experience. No production-grade persistence, automatic polling, or content rendering is required unless the project scope is formally expanded. The architecture must support future enhancement without requiring a full rewrite, but it must not be allowed to drift beyond the current phase without a clear requirement.

## Development Workflow
The project MUST follow a disciplined, phase-based workflow:

- Verify the backend and frontend start correctly with the agreed local port configuration.
- Confirm the frontend points to the correct backend API URL and that the CORS policy allows the frontend origin.
- Remove template demo pages and route conflicts before implementing project features.
- Keep the current work aligned to the active phase: MVP, Extended-MVP, or post-MVP.
- Review changes for security, maintainability, and code quality before merge.

This workflow exists to prevent avoidable defects, especially around routing conflicts, network configuration, and scope creep.

## Governance
This constitution governs project decisions, review expectations, and scope discipline. Any decision that conflicts with these principles must be resolved in favor of the more secure, simpler, and more maintainable option. Amendments require a documented rationale, a version update, and a review against the project goals, stack constraints, and MVP scope.

**Version**: 1.0.0 | **Ratified**: 2026-10-23 | **Last Amended**: 2026-10-23
