# Tasks: RSS Reader MVP

**Input**: Design documents from `/specs/001-rss-reader-mvp/`

**Prerequisites**: plan.md, spec.md, data-model.md, research.md, contracts/

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Create the minimal ASP.NET Core API and Blazor WebAssembly structure required for the MVP.

- [ ] T001 Create the required project folders and solution layout per the plan in backend/RSSFeedReader.Api, backend/RSSFeedReader.Api.Tests, frontend/RSSFeedReader.UI, and frontend/RSSFeedReader.UI.Tests
- [ ] T002 [P] Initialize the ASP.NET Core Web API and Blazor WebAssembly projects with the required .NET 10 dependencies in backend/RSSFeedReader.Api and frontend/RSSFeedReader.UI
- [ ] T003 [P] Configure backend startup, CORS, and frontend API base URL settings in backend/RSSFeedReader.Api/Program.cs and frontend/RSSFeedReader.UI/wwwroot/appsettings.json

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Build the shared validation and in-memory subscription infrastructure before the user stories begin.

**Checkpoint**: The API can validate input and hold a process-local subscription list before any story-specific UI work starts.

- [ ] T004 Create the `FeedSubscription` model with required URL and timestamp fields in backend/RSSFeedReader.Api/Models/FeedSubscription.cs
- [ ] T005 [P] Implement the in-memory subscription service with create/list behavior in backend/RSSFeedReader.Api/Services/SubscriptionService.cs
- [ ] T006 [P] Add URL validation and duplicate detection for nonblank absolute HTTP(S) URLs only in backend/RSSFeedReader.Api/Services/SubscriptionService.cs
- [ ] T007 Configure API registration and validation pipeline wiring for the backend in backend/RSSFeedReader.Api/Program.cs

---

## Phase 3: User Story 1 - Add a feed subscription (Priority: P1) 🎯 MVP

**Goal**: A user can enter a valid feed URL, submit it, and immediately see the new subscription in the list without leaving the page.

**Independent Test**: A user opens the subscription page, submits a valid URL, and sees the added entry appear immediately and without page refresh.

### Tests for User Story 1

- [ ] T008 [P] [US1] Add a unit test for valid and invalid URL validation in backend/RSSFeedReader.Api.Tests/SubscriptionServiceTests.cs
- [ ] T009 [P] [US1] Add API integration tests for successful create and validation failure in backend/RSSFeedReader.Api.Tests/SubscriptionsControllerTests.cs
- [ ] T010 [P] [US1] Add UI test coverage for the subscription form and inline validation in frontend/RSSFeedReader.UI.Tests/SubscriptionsPageTests.cs

### Implementation for User Story 1

- [ ] T011 [P] [US1] Implement the subscription request/response models and route contract for the API in backend/RSSFeedReader.Api/Contracts/SubscriptionRequest.cs and backend/RSSFeedReader.Api/Contracts/SubscriptionResponse.cs
- [ ] T012 [US1] Implement the `GET /api/subscriptions` and `POST /api/subscriptions` endpoints in backend/RSSFeedReader.Api/Controllers/SubscriptionsController.cs
- [ ] T013 [US1] Add validation and duplicate rejection logic with proper 400/409 responses in backend/RSSFeedReader.Api/Services/SubscriptionService.cs
- [ ] T014 [US1] Wire the API startup and CORS configuration so the frontend can call the backend in backend/RSSFeedReader.Api/Program.cs
- [ ] T015 [US1] Build the subscription input form, add action, and success/error feedback in frontend/RSSFeedReader.UI/Pages/Subscriptions.razor
- [ ] T016 [P] [US1] Implement the frontend API client and load the configured backend URL from frontend/RSSFeedReader.UI/Services/SubscriptionClient.cs and frontend/RSSFeedReader.UI/wwwroot/appsettings.json

**Checkpoint**: User Story 1 is functional and independently testable: users can add one valid feed URL and see it appear in the current list.

---

## Phase 4: User Story 2 - View subscription list (Priority: P1)

**Goal**: A user can confirm that the current list is visible, readable, and updated after each valid add action.

**Independent Test**: A user adds one or more valid URLs and sees each entry represented distinctly in the list after the action succeeds.

### Tests for User Story 2

- [ ] T017 [P] [US2] Add an API test covering the empty list and populated list response in backend/RSSFeedReader.Api.Tests/SubscriptionsControllerTests.cs
- [ ] T018 [P] [US2] Add a UI test covering list rendering and refresh after a successful add in frontend/RSSFeedReader.UI.Tests/SubscriptionsPageTests.cs

### Implementation for User Story 2

- [ ] T019 [P] [US2] Return insertion-ordered subscription records from the API in backend/RSSFeedReader.Api/Controllers/SubscriptionsController.cs
- [ ] T020 [US2] Render the subscription list and update it after each successful add in frontend/RSSFeedReader.UI/Pages/Subscriptions.razor
- [ ] T021 [P] [US2] Show distinct list entries with a clear user-visible status message for success and rejected inputs in frontend/RSSFeedReader.UI/Pages/Subscriptions.razor

**Checkpoint**: User Story 2 is independently functional: the UI shows a stable, current subscription list on the page.

---

## Phase 5: User Story 3 - Keep the MVP intentionally simple (Priority: P2)

**Goal**: Preserve the prove-it scope by keeping the app limited to add/list behavior and explicitly excluding feed fetching, persistence, and item rendering.

**Independent Test**: A reviewer confirms that the app supports adding and listing subscriptions only, with no feed fetching or content display required.

### Implementation for User Story 3

- [ ] T022 [US3] Verify that the backend and frontend do not introduce feed-fetching, parsing, persistence, removal, or item-display logic in backend/RSSFeedReader.Api and frontend/RSSFeedReader.UI
- [ ] T023 [P] [US3] Add a brief scope guardrail and validation note in specs/001-rss-reader-mvp/quickstart.md to confirm the app remains intentionally minimal

**Checkpoint**: The MVP scope remains clearly bounded to subscription management and nothing beyond it.

---

## Phase 6: Polish & Cross-Cutting Concerns

**Purpose**: Final validation, cleanup, and project hygiene for the delivery-ready MVP.

- [ ] T024 [P] Run the backend unit and API validation suite in backend/RSSFeedReader.Api.Tests
- [ ] T025 [P] Run the frontend build and browser validation workflow described in specs/001-rss-reader-mvp/quickstart.md
- [ ] T026 Check routing, CORS, and appsettings alignment across backend/RSSFeedReader.Api/Program.cs and frontend/RSSFeedReader.UI/wwwroot/appsettings.json
- [ ] T027 Remove template demo pages, default navigation, and unrelated sample content from frontend/RSSFeedReader.UI so the subscription page is the only primary experience

---

## Dependencies & Execution Order

### Phase Dependencies

- **Setup (Phase 1)**: No dependencies; can start immediately.
- **Foundational (Phase 2)**: Depends on Setup completion; blocks all user story work.
- **User Story 1 (Phase 3)**: Depends on Foundational completion; is the MVP milestone.
- **User Story 2 (Phase 4)**: Depends on User Story 1 and can follow once the list API and UI are stable.
- **User Story 3 (Phase 5)**: Depends on User Story 1 and 2 completion; confirms MVP scope discipline.
- **Polish (Phase 6)**: Depends on all wanted features and validation checks.

### User Story Dependencies

- **US1**: No dependency on other user stories; it is the core MVP workflow.
- **US2**: Depends on US1 for the add-to-list behavior, but remains independently testable once the API returns list data.
- **US3**: Depends on the implemented MVP shape but does not add runtime functionality.

### Parallel Opportunities

- Setup tasks T001-T003 can run in parallel.
- Foundational tasks T005-T007 can run in parallel after T004 establishes the model contract.
- US1 tests T008-T010 can be developed in parallel.
- US2 tests T017-T018 can be developed in parallel.
- US1 implementation tasks T011-T016 can be split across the API and frontend workstreams in parallel.
- US2 implementation tasks T019-T021 can run in parallel once the API behavior is confirmed.
- Final validation tasks T024-T027 are parallelizable after all feature work is complete.

---

## Parallel Example: User Story 1

```bash
# API and frontend can advance together once discovery and setup are done
Task: "Add API integration tests for successful create and validation failure in backend/RSSFeedReader.Api.Tests/SubscriptionsControllerTests.cs"
Task: "Add UI test coverage for the subscription form and inline validation in frontend/RSSFeedReader.UI.Tests/SubscriptionsPageTests.cs"
Task: "Implement the subscription request/response models and route contract for the API in backend/RSSFeedReader.Api/Contracts/SubscriptionRequest.cs and backend/RSSFeedReader.Api/Contracts/SubscriptionResponse.cs"
Task: "Build the subscription input form, add action, and success/error feedback in frontend/RSSFeedReader.UI/Pages/Subscriptions.razor"
```

---

## Implementation Strategy

### MVP First (User Story 1 Only)

1. Complete Phase 1: Setup.
2. Complete Phase 2: Foundational.
3. Complete Phase 3: User Story 1.
4. Validate the scenario end-to-end before moving on.
5. Stop after a clean MVP demo if broader roadmap work is intentionally deferred.

### Incremental Delivery

1. Setup + Foundational -> backend and frontend ready for feature work.
2. Add US1 -> validate add + list flow.
3. Add US2 -> validate list rendering and stale-state handling.
4. Confirm US3 scope guardrails -> keep the project intentionally minimal.
5. Run final polish and validation as a closing gate.

---

## Notes

- All tasks follow the required checklist format with a checkbox, task ID, optional [P] marker, and exact file paths.
- Story-specific tasks are labeled with [US1], [US2], or [US3].
- Foundational tasks are deliberately shared and blocking for all user stories.
- The implementation MUST reject blank, malformed, unsupported, or duplicate URLs and must not fetch feed content or parse feed XML/Atom during this MVP.
