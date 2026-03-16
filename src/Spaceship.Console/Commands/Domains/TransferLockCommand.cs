using System.ComponentModel;
using Spaceship.Console.Infrastructure;
using Spectre.Console.Cli;

namespace Spaceship.Console.Commands.Domains;

public sealed class TransferLockSettings : GlobalSettings
{
    [CommandArgument(0, "<domain>")]
    [Description("Domain name")]
    public required string Domain { get; set; }

    [CommandOption("--lock")]
    [Description("Lock the domain")]
    public bool Lock { get; set; }

    [CommandOption("--unlock")]
    [Description("Unlock the domain")]
    public bool Unlock { get; set; }
}

[Description("Update domain transfer lock")]
public sealed class TransferLockCommand : SpaceshipCommand<TransferLockSettings>
{
    protected override async Task<object> ExecuteAsync(SpaceshipApiClient client, TransferLockSettings settings)
    {
        if (!settings.Lock && !settings.Unlock)
            throw new SpaceshipException("Specify --lock or --unlock.");
        if (settings.Lock && settings.Unlock)
            throw new SpaceshipException("Cannot specify both --lock and --unlock.");

        var result = await client.PutAsync($"/domains/{settings.Domain}/transfer/lock", new
        {
            isLocked = settings.Lock
        });
        return ToObject(result);
    }
}
