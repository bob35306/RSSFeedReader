# Subscriptions API Contract

Base path: `/api/subscriptions`

The API provides in-process subscription management only. It MUST NOT fetch the submitted URL or parse RSS/Atom content.

## `GET /api/subscriptions`

Returns the current process-local list in insertion order.

**Response: `200 OK`**

```json
[
  {
    "id": "4f78e85e-60a9-4d3b-bf3a-c8f7ca7c7298",
    "url": "https://example.com/feed.xml",
    "createdAt": "2026-10-23T12:00:00Z"
  }
]
```

An empty list is returned as `[]`.

## `POST /api/subscriptions`

Adds one subscription.

**Request**

```json
{
  "url": "https://example.com/feed.xml"
}
```

The API trims surrounding whitespace and requires a non-empty absolute HTTP or HTTPS URL. It rejects duplicates according to normalized URL comparison.

**Response: `201 Created`**

Returns the created `FeedSubscription` representation and a `Location` header for `/api/subscriptions/{id}`. A per-item GET route is not required; clients should use the collection endpoint to reload the list.

**Response: `400 Bad Request`**

Returned for missing, blank, malformed, or non-HTTP(S) URL input. Use a stable validation problem response with a field-level message.

**Response: `409 Conflict`**

Returned when the normalized URL already exists. Include a user-readable message; do not add another list entry.

## Cross-origin and runtime configuration

- The frontend API base URL is read from `wwwroot/appsettings.json`; do not hardcode it in page code.
- Configure backend CORS to allow the actual frontend development origins.
- Keep launch profile ports and configured API URL aligned. See `StakeholderDocuments/TechStack.md` for the initial local port values.