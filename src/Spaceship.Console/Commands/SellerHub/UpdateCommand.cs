using System.ComponentModel;
using System.Text.Json;
using Spaceship.Console.Infrastructure;
using Spectre.Console.Cli;

namespace Spaceship.Console.Commands.SellerHub;

public sealed class UpdateSettings : GlobalSettings
{
    [CommandArgument(0, "<id>")]
    [Description("SellerHub domain ID")]
    public required string Id { get; set; }

    [CommandOption("--file <FILE>")]
    [Description("JSON file with fields to update (or pipe via stdin)")]
    public string? File { get; set; }
}

[Description("Update SellerHub domain")]
public sealed class UpdateCommand : SpaceshipCommand<UpdateSettings>
{
    protected override async Task<object> ExecuteAsync(SpaceshipApiClient client, UpdateSettings settings)
    {
        string json;
        if (!string.IsNullOrWhiteSpace(settings.File))
            json = await System.IO.File.ReadAllTextAsync(settings.File);
        else if (!System.Console.IsInputRedirected)
            throw new SpaceshipException("Provide update fields via stdin or --file.");
        else
            json = await System.Console.In.ReadToEndAsync();

        var body = JsonSerializer.Deserialize<JsonElement>(json);
        var result = await client.PatchAsync($"/sellerhub/domains/{settings.Id}", ToObject(body));
        return ToObject(result);
    }
}
