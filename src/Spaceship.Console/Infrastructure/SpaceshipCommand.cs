using System.Text.Json;
using Spectre.Console.Cli;

namespace Spaceship.Console.Infrastructure;

public abstract class SpaceshipCommand<TSettings> : AsyncCommand<TSettings> where TSettings : GlobalSettings
{
    protected sealed override async Task<int> ExecuteAsync(CommandContext context, TSettings settings, CancellationToken cancellation)
    {
        try
        {
            var (key, secret) = settings.ResolveCredentials();
            using var client = new SpaceshipApiClient(key, secret, settings.Verbose);
            var result = await ExecuteAsync(client, settings);
            OutputHelper.Write(result, settings.Format);
            return 0;
        }
        catch (SpaceshipApiException ex)
        {
            OutputHelper.WriteError(ex.Message, ex.StatusCode);
            return ex.StatusCode;
        }
        catch (SpaceshipException ex)
        {
            OutputHelper.WriteError(ex.Message, 1);
            return 1;
        }
        catch (HttpRequestException ex)
        {
            OutputHelper.WriteError($"Connection failed: {ex.Message}", 2);
            return 2;
        }
    }

    protected abstract Task<object> ExecuteAsync(SpaceshipApiClient client, TSettings settings);

    protected static object ToObject(JsonElement element) => ConvertElement(element);

    private static object ConvertElement(JsonElement element) => element.ValueKind switch
    {
        JsonValueKind.Object => element.EnumerateObject()
            .ToDictionary(p => p.Name, p => ConvertElement(p.Value)),
        JsonValueKind.Array => element.EnumerateArray()
            .Select(ConvertElement).ToList(),
        JsonValueKind.String => element.GetString()!,
        JsonValueKind.Number => element.TryGetInt64(out var l) ? l : element.GetDouble(),
        JsonValueKind.True => true,
        JsonValueKind.False => false,
        _ => (object)null!
    };
}
