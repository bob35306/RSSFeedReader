namespace RSSFeedReader.Api.Contracts;

public sealed record SubscriptionResponse(Guid Id, string Url, DateTimeOffset CreatedAt);