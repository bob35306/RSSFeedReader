using Microsoft.AspNetCore.Mvc;
using RSSFeedReader.Api.Contracts;
using RSSFeedReader.Api.Models;
using RSSFeedReader.Api.Services;

namespace RSSFeedReader.Api.Controllers;

[ApiController]
[Route("api/subscriptions")]
public sealed class SubscriptionsController : ControllerBase
{
    private readonly SubscriptionService _subscriptions;

    public SubscriptionsController(SubscriptionService subscriptions)
    {
        _subscriptions = subscriptions;
    }

    [HttpGet]
    public ActionResult<IReadOnlyList<SubscriptionResponse>> GetAll()
    {
        var subscriptions = _subscriptions.GetAll().Select(ToResponse).ToArray();
        return Ok(subscriptions);
    }

    [HttpPost]
    public ActionResult<SubscriptionResponse> Create(SubscriptionRequest request)
    {
        var result = _subscriptions.TryAdd(request.Url);
        if (result.IsDuplicate)
        {
            return Conflict(new ProblemDetails
            {
                Status = StatusCodes.Status409Conflict,
                Title = "Subscription already exists",
                Detail = result.Error
            });
        }

        if (result.Subscription is null)
        {
            return BadRequest(new ValidationProblemDetails(new Dictionary<string, string[]>
            {
                ["url"] = [result.Error ?? "Enter a valid feed URL."]
            })
            {
                Title = "The feed URL is invalid.",
                Status = StatusCodes.Status400BadRequest
            });
        }

        var response = ToResponse(result.Subscription);
        return Created($"/api/subscriptions/{response.Id}", response);
    }

    private static SubscriptionResponse ToResponse(FeedSubscription subscription) =>
        new(subscription.Id, subscription.Url, subscription.CreatedAt);
}