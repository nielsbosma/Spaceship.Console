using System.ComponentModel;
using System.Text.Json;
using Spaceship.Console.Infrastructure;
using Spectre.Console.Cli;

namespace Spaceship.Console.Commands.SellerHub;

public sealed class CreateSettings : GlobalSettings
{
    [CommandOption("--file <FILE>")]
    [Description("JSON file with domain details (or pipe via stdin)")]
    public string? File { get; set; }
}

[Description("Create SellerHub domain")]
public sealed class CreateCommand : SpaceshipCommand<CreateSettings>
{
    protected override async Task<object> ExecuteAsync(SpaceshipApiClient client, CreateSettings settings)
    {
        string json;
        if (!string.IsNullOrWhiteSpace(settings.File))
            json = await System.IO.File.ReadAllTextAsync(settings.File);
        else if (!System.Console.IsInputRedirected)
            throw new SpaceshipException("Provide domain details via stdin or --file.");
        else
            json = await System.Console.In.ReadToEndAsync();

        var body = JsonSerializer.Deserialize<JsonElement>(json);
        var result = await client.PostAsync("/sellerhub/domains", ToObject(body));
        return ToObject(result);
    }
}
