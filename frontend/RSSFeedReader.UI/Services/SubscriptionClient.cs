using System.Net.Http.Json;
using RSSFeedReader.UI.Contracts;

namespace RSSFeedReader.UI.Services;

public sealed class SubscriptionClient(HttpClient httpClient)
{
    public async Task<IReadOnlyList<SubscriptionResponse>> GetAllAsync()
    {
        return await httpClient.GetFromJsonAsync<SubscriptionResponse[]>("subscriptions") ?? [];
    }

    public async Task<SubscriptionResponse> AddAsync(string url)
    {
        using var response = await httpClient.PostAsJsonAsync("subscriptions", new SubscriptionRequest(url));
        if (!response.IsSuccessStatusCode)
        {
            var problem = await response.Content.ReadFromJsonAsync<SubscriptionProblem>();
            var message = problem?.Errors?.Values.FirstOrDefault()?.FirstOrDefault()
                ?? problem?.Detail
                ?? problem?.Title
                ?? "The subscription could not be added.";
            throw new SubscriptionClientException(message);
        }

        return await response.Content.ReadFromJsonAsync<SubscriptionResponse>()
            ?? throw new SubscriptionClientException("The API returned an empty subscription response.");
    }

    private sealed record SubscriptionProblem(
        string? Title,
        string? Detail,
        Dictionary<string, string[]>? Errors);
}

public sealed class SubscriptionClientException(string message) : Exception(message);