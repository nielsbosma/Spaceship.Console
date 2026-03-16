using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Spaceship.Console.Infrastructure;

public sealed class SpaceshipApiClient : IDisposable
{
    private const string BaseUrl = "https://spaceship.dev/api/v1/";

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    private readonly HttpClient _http;
    private readonly bool _verbose;

    public SpaceshipApiClient(string apiKey, string apiSecret, bool verbose = false)
    {
        _verbose = verbose;
        _http = new HttpClient { BaseAddress = new Uri(BaseUrl) };
        _http.DefaultRequestHeaders.Add("X-API-Key", apiKey);
        _http.DefaultRequestHeaders.Add("X-API-Secret", apiSecret);
        _http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public async Task<JsonElement> GetAsync(string path)
    {
        var p = NormalizePath(path);
        LogRequest("GET", p);
        var response = await _http.GetAsync(p);
        return await HandleResponseAsync(response);
    }

    public async Task<JsonElement> PostAsync(string path, object? body = null)
    {
        var p = NormalizePath(path);
        LogRequest("POST", p);
        var content = body is not null
            ? new StringContent(JsonSerializer.Serialize(body, JsonOptions), Encoding.UTF8, "application/json")
            : null;
        var response = await _http.PostAsync(p, content);
        return await HandleResponseAsync(response);
    }

    public async Task<JsonElement> PutAsync(string path, object? body = null)
    {
        var p = NormalizePath(path);
        LogRequest("PUT", p);
        var content = body is not null
            ? new StringContent(JsonSerializer.Serialize(body, JsonOptions), Encoding.UTF8, "application/json")
            : null;
        var response = await _http.PutAsync(p, content);
        return await HandleResponseAsync(response);
    }

    public async Task<JsonElement> PatchAsync(string path, object? body = null)
    {
        var p = NormalizePath(path);
        LogRequest("PATCH", p);
        var content = body is not null
            ? new StringContent(JsonSerializer.Serialize(body, JsonOptions), Encoding.UTF8, "application/json")
            : null;
        var request = new HttpRequestMessage(HttpMethod.Patch, p) { Content = content };
        var response = await _http.SendAsync(request);
        return await HandleResponseAsync(response);
    }

    public async Task<JsonElement> DeleteAsync(string path, object? body = null)
    {
        var p = NormalizePath(path);
        LogRequest("DELETE", p);
        HttpResponseMessage response;
        if (body is not null)
        {
            var content = new StringContent(JsonSerializer.Serialize(body, JsonOptions), Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Delete, p) { Content = content };
            response = await _http.SendAsync(request);
        }
        else
        {
            response = await _http.DeleteAsync(p);
        }
        return await HandleResponseAsync(response);
    }

    private static string NormalizePath(string path) => path.TrimStart('/');

    private async Task<JsonElement> HandleResponseAsync(HttpResponseMessage response)
    {
        LogResponse(response);
        var body = await response.Content.ReadAsStringAsync();

        if (response.StatusCode == HttpStatusCode.TooManyRequests)
        {
            var retryAfter = response.Headers.RetryAfter?.Delta?.TotalSeconds ?? 60;
            throw new SpaceshipApiException($"Rate limited (429). Retry after {retryAfter:F0}s", 2);
        }

        if (!response.IsSuccessStatusCode)
        {
            var message = TryExtractErrorMessage(body) ?? $"API error: {response.StatusCode}";
            throw new SpaceshipApiException(message, response.StatusCode >= HttpStatusCode.InternalServerError ? 2 : 1);
        }

        if (string.IsNullOrWhiteSpace(body))
            return JsonSerializer.SerializeToElement(new { success = true });

        return JsonSerializer.Deserialize<JsonElement>(body);
    }

    private static string? TryExtractErrorMessage(string body)
    {
        try
        {
            using var doc = JsonDocument.Parse(body);
            var root = doc.RootElement;
            if (root.TryGetProperty("message", out var msg))
                return msg.GetString();
            if (root.TryGetProperty("error", out var err))
                return err.GetString();
            if (root.TryGetProperty("detail", out var detail))
                return detail.GetString();
            if (root.TryGetProperty("title", out var title))
            {
                var text = title.GetString() ?? "Validation error";
                if (root.TryGetProperty("errors", out var errors) && errors.ValueKind == JsonValueKind.Object)
                {
                    var details = errors.EnumerateObject()
                        .SelectMany(p => p.Value.EnumerateArray().Select(v => $"{p.Name}: {v.GetString()}"));
                    text += " - " + string.Join("; ", details);
                }
                return text;
            }
        }
        catch { }
        return null;
    }

    private void LogRequest(string method, string path)
    {
        if (_verbose)
            System.Console.Error.WriteLine($">> {method} {BaseUrl}{path}");
    }

    private void LogResponse(HttpResponseMessage response)
    {
        if (_verbose)
            System.Console.Error.WriteLine($"<< {(int)response.StatusCode} {response.StatusCode}");
    }

    public void Dispose() => _http.Dispose();
}
