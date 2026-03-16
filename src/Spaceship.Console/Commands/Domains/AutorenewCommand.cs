using System.ComponentModel;
using Spaceship.Console.Infrastructure;
using Spectre.Console.Cli;

namespace Spaceship.Console.Commands.Domains;

public sealed class AutorenewSettings : GlobalSettings
{
    [CommandArgument(0, "<domain>")]
    [Description("Domain name")]
    public required string Domain { get; set; }

    [CommandOption("--enable")]
    [Description("Enable auto-renewal")]
    public bool Enable { get; set; }

    [CommandOption("--disable")]
    [Description("Disable auto-renewal")]
    public bool Disable { get; set; }
}

[Description("Update domain auto-renewal")]
public sealed class AutorenewCommand : SpaceshipCommand<AutorenewSettings>
{
    protected override async Task<object> ExecuteAsync(SpaceshipApiClient client, AutorenewSettings settings)
    {
        if (!settings.Enable && !settings.Disable)
            throw new SpaceshipException("Specify --enable or --disable.");
        if (settings.Enable && settings.Disable)
            throw new SpaceshipException("Cannot specify both --enable and --disable.");

        var result = await client.PutAsync($"/domains/{settings.Domain}/autorenew", new
        {
            isEnabled = settings.Enable
        });
        return ToObject(result);
    }
}
