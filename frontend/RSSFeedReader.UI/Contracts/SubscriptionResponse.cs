namespace RSSFeedReader.UI.Contracts;

public sealed record SubscriptionResponse(Guid Id, string Url, DateTimeOffset CreatedAt);