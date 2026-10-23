# Quickstart: RSS Reader MVP Validation

## Prerequisites

- Install the .NET 10 SDK and confirm `dotnet --version` reports the selected SDK.
- Use a current browser and a local development environment on Windows, macOS, or Linux.
- No database, feed URL, or network access to feed sources is required for the MVP workflow.

## Configure and run

1. Confirm backend launch settings use `http://localhost:5151` and frontend launch settings use `http://localhost:5213` (or update all related configuration consistently).
2. Set `frontend/RSSFeedReader.UI/wwwroot/appsettings.json` `ApiBaseUrl` to `http://localhost:5151/api/`.
3. Confirm the API CORS policy allows the frontend origin from its launch settings.
4. Build and test the solution:

   ```powershell
   dotnet build
   dotnet test
   ```

5. Run the API and frontend in separate terminals:

   ```powershell
   dotnet run --project backend/RSSFeedReader.Api
   dotnet run --project frontend/RSSFeedReader.UI
   ```

6. Open the frontend URL printed by the UI process.

## Acceptance checks

1. On first load, the subscription list is visible and empty.
2. Submit `https://devblogs.microsoft.com/dotnet/feed/`; the entry appears without navigating away.
3. Refresh the browser while the API remains running; the entry is still returned from the API's in-memory list.
4. Submit blank input, a malformed URL, and a non-HTTP(S) scheme; each is rejected with a visible message and the list remains unchanged.
5. Submit the same valid URL again; the API rejects the duplicate and the list still contains exactly one matching subscription.
6. Stop and restart the API; the list is empty, confirming that MVP storage is not persistent.
7. Confirm no feed request is made to the submitted URL and that no feed items are rendered.

## Focused tests

- Unit-test URL normalization/validation and duplicate detection in the subscription service.
- Integration-test list, successful add, invalid input, and duplicate responses through the API.
- Verify that the frontend displays API validation failures and reloads the collection after a successful add.
- Before building the UI, remove template `Home.razor`, `Counter.razor`, and `Weather.razor`, remove their navigation links, and verify that only one page defines the `/` route.
- In browser DevTools, verify there are no API connection, CORS, or ambiguous-route errors.

See [data-model.md](data-model.md) and [contracts/subscriptions-api.md](contracts/subscriptions-api.md) for entity and endpoint details.