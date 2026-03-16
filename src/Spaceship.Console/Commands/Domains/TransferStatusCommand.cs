using System.ComponentModel;
using Spaceship.Console.Infrastructure;
using Spectre.Console.Cli;

namespace Spaceship.Console.Commands.Domains;

public sealed class TransferStatusSettings : GlobalSettings
{
    [CommandArgument(0, "<domain>")]
    [Description("Domain name")]
    public required string Domain { get; set; }
}

[Description("Get domain transfer status")]
public sealed class TransferStatusCommand : SpaceshipCommand<TransferStatusSettings>
{
    protected override async Task<object> ExecuteAsync(SpaceshipApiClient client, TransferStatusSettings settings)
    {
        var result = await client.GetAsync($"/domains/{settings.Domain}/transfer");
        return ToObject(result);
    }
}
