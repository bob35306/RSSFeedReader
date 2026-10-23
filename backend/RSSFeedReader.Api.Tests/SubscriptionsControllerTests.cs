using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using RSSFeedReader.Api.Contracts;
using Xunit;

namespace RSSFeedReader.Api.Tests;

public sealed class SubscriptionsControllerTests : IDisposable
{
    private readonly WebApplicationFactory<Program> _factory = new();
    private readonly HttpClient _client;

    public SubscriptionsControllerTests()
    {
        _client = _factory.CreateClient();
    }

    public void Dispose()
    {
        _client.Dispose();
        _factory.Dispose();
    }

    [Fact]
    public async Task Post_ValidUrl_ReturnsCreatedSubscription()
    {
        var response = await _client.PostAsJsonAsync("/api/subscriptions", new SubscriptionRequest("https://example.com/feed.xml"));

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.StartsWith("/api/subscriptions/", response.Headers.Location?.OriginalString);
        var subscription = await response.Content.ReadFromJsonAsync<SubscriptionResponse>();
        Assert.NotNull(subscription);
        Assert.Equal("https://example.com/feed.xml", subscription.Url);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("ftp://example.com/feed.xml")]
    public async Task Post_InvalidUrl_ReturnsFieldValidationError(string? url)
    {
        var response = await _client.PostAsJsonAsync("/api/subscriptions", new SubscriptionRequest(url));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var body = await response.Content.ReadAsStringAsync();
        Assert.Contains("url", body, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task Get_ReturnsEmptyThenInsertionOrderedSubscriptions()
    {
        var initial = await _client.GetFromJsonAsync<List<SubscriptionResponse>>("/api/subscriptions");
        Assert.NotNull(initial);
        Assert.Empty(initial);

        await _client.PostAsJsonAsync("/api/subscriptions", new SubscriptionRequest("https://example.com/one"));
        await _client.PostAsJsonAsync("/api/subscriptions", new SubscriptionRequest("https://example.com/two"));

        var subscriptions = await _client.GetFromJsonAsync<List<SubscriptionResponse>>("/api/subscriptions");
        Assert.NotNull(subscriptions);
        Assert.Equal(["https://example.com/one", "https://example.com/two"], subscriptions.Select(item => item.Url));
    }

    [Fact]
    public async Task Post_DuplicateUrl_ReturnsConflictWithoutAddingSecondEntry()
    {
        await _client.PostAsJsonAsync("/api/subscriptions", new SubscriptionRequest("https://example.com/feed"));

        var duplicate = await _client.PostAsJsonAsync("/api/subscriptions", new SubscriptionRequest("https://EXAMPLE.com:443/feed"));
        var subscriptions = await _client.GetFromJsonAsync<List<SubscriptionResponse>>("/api/subscriptions");

        Assert.Equal(HttpStatusCode.Conflict, duplicate.StatusCode);
        Assert.NotNull(subscriptions);
        Assert.Single(subscriptions);
    }
}