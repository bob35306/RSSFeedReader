using RSSFeedReader.Api.Models;

namespace RSSFeedReader.Api.Services;

public sealed class SubscriptionService
{
    private readonly object _sync = new();
    private readonly List<FeedSubscription> _subscriptions = [];
    private readonly HashSet<string> _normalizedUrls = new(StringComparer.Ordinal);

    public IReadOnlyList<FeedSubscription> GetAll()
    {
        lock (_sync)
        {
            return _subscriptions.ToArray();
        }
    }

    public SubscriptionAddResult TryAdd(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return SubscriptionAddResult.Invalid("A feed URL is required.");
        }

        var trimmed = value.Trim();
        if (!Uri.TryCreate(trimmed, UriKind.Absolute, out var uri) ||
            (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps) ||
            string.IsNullOrWhiteSpace(uri.Host))
        {
            return SubscriptionAddResult.Invalid("Enter an absolute HTTP or HTTPS URL.");
        }

        var normalizedUrl = uri.GetComponents(UriComponents.AbsoluteUri, UriFormat.UriEscaped);

        lock (_sync)
        {
            if (!_normalizedUrls.Add(normalizedUrl))
            {
                return SubscriptionAddResult.Duplicate("This URL is already subscribed.");
            }

            var subscription = new FeedSubscription(Guid.NewGuid(), normalizedUrl, DateTimeOffset.UtcNow);
            _subscriptions.Add(subscription);
            return SubscriptionAddResult.Added(subscription);
        }
    }
}

public sealed record SubscriptionAddResult(
    FeedSubscription? Subscription,
    string? Error,
    bool IsDuplicate)
{
    public static SubscriptionAddResult Added(FeedSubscription subscription) => new(subscription, null, false);

    public static SubscriptionAddResult Invalid(string error) => new(null, error, false);

    public static SubscriptionAddResult Duplicate(string error) => new(null, error, true);
}