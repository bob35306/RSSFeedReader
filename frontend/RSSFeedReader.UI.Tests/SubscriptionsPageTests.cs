using System.Net;
using System.Net.Http.Json;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using RSSFeedReader.UI.Contracts;
using RSSFeedReader.UI.Pages;
using RSSFeedReader.UI.Services;
using Xunit;

namespace RSSFeedReader.UI.Tests;

public sealed class SubscriptionsPageTests
{
    [Fact]
    public async Task InvalidUrl_ShowsInlineValidation()
    {
        using var context = CreateContext(new SubscriptionTestHandler());

        var page = context.RenderComponent<Subscriptions>();
        page.Find("input[type='url']").Change("ftp://example.com/feed.xml");
        await page.Find("form").TriggerEventAsync("onsubmit", EventArgs.Empty);

        Assert.Contains("Enter an absolute HTTP or HTTPS URL.", page.Markup);
    }

    [Fact]
    public async Task Page_RendersExistingEntriesAndRefreshesAfterAdd()
    {
        var handler = new SubscriptionTestHandler(
        [
            new SubscriptionResponse(Guid.NewGuid(), "https://example.com/first", DateTimeOffset.UtcNow)
        ]);
        using var context = CreateContext(handler);

        var page = context.RenderComponent<Subscriptions>();
        page.WaitForAssertion(() => Assert.Contains("https://example.com/first", page.Markup));
        page.Find("input[type='url']").Change("https://example.com/second");
        await page.Find("form").TriggerEventAsync("onsubmit", EventArgs.Empty);
        page.WaitForAssertion(() => Assert.Contains("https://example.com/second", page.Markup));

        Assert.Contains("https://example.com/first", page.Markup);
        Assert.Contains("Subscription added.", page.Markup);
    }

    private static TestContext CreateContext(SubscriptionTestHandler handler)
    {
        var context = new TestContext();
        context.Services.AddScoped(_ => new HttpClient(handler)
        {
            BaseAddress = new Uri("http://localhost/api/")
        });
        context.Services.AddScoped<SubscriptionClient>();
        return context;
    }

    private sealed class SubscriptionTestHandler(List<SubscriptionResponse>? initial = null) : HttpMessageHandler
    {
        private readonly List<SubscriptionResponse> _subscriptions = initial ?? [];

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.Method == HttpMethod.Get)
            {
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = JsonContent.Create(_subscriptions)
                };
            }

            var input = await request.Content!.ReadFromJsonAsync<SubscriptionRequest>(cancellationToken: cancellationToken);
            var subscription = new SubscriptionResponse(Guid.NewGuid(), input!.Url!, DateTimeOffset.UtcNow);
            _subscriptions.Add(subscription);
            return new HttpResponseMessage(HttpStatusCode.Created)
            {
                Content = JsonContent.Create(subscription)
            };
        }
    }
}