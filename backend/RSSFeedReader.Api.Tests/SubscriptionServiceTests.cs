using RSSFeedReader.Api.Services;
using Xunit;

namespace RSSFeedReader.Api.Tests;

public sealed class SubscriptionServiceTests
{
    [Theory]
    [InlineData("https://example.com/feed.xml")]
    [InlineData("http://localhost:8080/rss")]
    public void TryAdd_AcceptsAbsoluteHttpUrls(string url)
    {
        var service = new SubscriptionService();

        var result = service.TryAdd(url);

        Assert.NotNull(result.Subscription);
        Assert.Equal(url, result.Subscription.Url);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("not a url")]
    [InlineData("ftp://example.com/feed.xml")]
    [InlineData("http:///feed.xml")]
    public void TryAdd_RejectsInvalidUrls(string? url)
    {
        var service = new SubscriptionService();

        var result = service.TryAdd(url);

        Assert.Null(result.Subscription);
        Assert.False(result.IsDuplicate);
        Assert.NotNull(result.Error);
        Assert.Empty(service.GetAll());
    }

    [Fact]
    public void TryAdd_RejectsNormalizedDuplicate()
    {
        var service = new SubscriptionService();
        var first = service.TryAdd("https://EXAMPLE.com:443/feed.xml");

        var duplicate = service.TryAdd("HTTPS://example.com/feed.xml");

        Assert.NotNull(first.Subscription);
        Assert.True(duplicate.IsDuplicate);
        Assert.Single(service.GetAll());
    }
}