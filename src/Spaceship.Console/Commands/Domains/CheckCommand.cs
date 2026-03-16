using System.ComponentModel;
using Spaceship.Console.Infrastructure;
using Spectre.Console.Cli;

namespace Spaceship.Console.Commands.Domains;

public sealed class CheckSettings : GlobalSettings
{
    [CommandArgument(0, "<domain>")]
    [Description("Domain name to check")]
    public required string Domain { get; set; }
}

[Description("Check domain availability")]
public sealed class CheckCommand : SpaceshipCommand<CheckSettings>
{
    protected override async Task<object> ExecuteAsync(SpaceshipApiClient client, CheckSettings settings)
    {
        var result = await client.GetAsync($"/domains/{settings.Domain}/available");
        return ToObject(result);
    }
}
