using System.ComponentModel;
using Spaceship.Console.Infrastructure;
using Spectre.Console.Cli;

namespace Spaceship.Console.Commands.Domains;

public sealed class CheckBatchSettings : GlobalSettings
{
    [CommandArgument(0, "<domains>")]
    [Description("Comma-separated domain names to check (max 20)")]
    public required string Domains { get; set; }
}

[Description("Check availability of multiple domains")]
public sealed class CheckBatchCommand : SpaceshipCommand<CheckBatchSettings>
{
    protected override async Task<object> ExecuteAsync(SpaceshipApiClient client, CheckBatchSettings settings)
    {
        var domains = settings.Domains.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (domains.Length > 20)
            throw new SpaceshipException("Maximum 20 domains per batch check.");

        var result = await client.PostAsync("/domains/available", new { domains });
        return ToObject(result);
    }
}
