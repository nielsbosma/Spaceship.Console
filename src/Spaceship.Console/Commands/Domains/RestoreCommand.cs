using System.ComponentModel;
using Spaceship.Console.Infrastructure;
using Spectre.Console.Cli;

namespace Spaceship.Console.Commands.Domains;

public sealed class RestoreSettings : GlobalSettings
{
    [CommandArgument(0, "<domain>")]
    [Description("Domain name to restore")]
    public required string Domain { get; set; }
}

[Description("Restore a deleted domain")]
public sealed class RestoreCommand : SpaceshipCommand<RestoreSettings>
{
    protected override async Task<object> ExecuteAsync(SpaceshipApiClient client, RestoreSettings settings)
    {
        var result = await client.PostAsync($"/domains/{settings.Domain}/restore");
        return ToObject(result);
    }
}
