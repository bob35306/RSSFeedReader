namespace RSSFeedReader.Api.Models;

public sealed record FeedSubscription(Guid Id, string Url, DateTimeOffset CreatedAt);