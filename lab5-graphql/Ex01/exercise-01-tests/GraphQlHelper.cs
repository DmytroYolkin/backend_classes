using System.Net.Http.Json;
using System.Text.Json;

namespace Ex01.Tests;

public static class GraphQlHelper
{
    public static async Task<JsonDocument> ExecuteAsync(
        HttpClient client,
        string query,
        object? variables = null)
    {
        var request = new { query, variables };
        var response = await client.PostAsJsonAsync("/graphql", request);
        var json = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException(
                $"GraphQL request failed with {(int)response.StatusCode}: {json}");

        return JsonDocument.Parse(json);
    }

    /// <summary>Returns the error code from errors[index].extensions.code or errors[index].code</summary>
    public static string? GetErrorCode(JsonElement error)
    {
        if (error.TryGetProperty("extensions", out var ext) &&
            ext.TryGetProperty("code", out var extCode))
            return extCode.GetString();

        if (error.TryGetProperty("code", out var directCode))
            return directCode.GetString();

        return null;
    }
}
