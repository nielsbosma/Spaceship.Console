using System.ComponentModel;
using Spaceship.Console.Infrastructure;
using Spectre.Console.Cli;

namespace Spaceship.Console.Commands.Domains;

public sealed class RenewSettings : GlobalSettings
{
    [CommandArgument(0, "<domain>")]
    [Description("Domain name to renew")]
    public required string Domain { get; set; }

    [CommandOption("--years <YEARS>")]
    [Description("Renewal period (1-10)")]
    [DefaultValue(1)]
    public int Years { get; set; } = 1;

    [CommandOption("--expiration-date <DATE>")]
    [Description("Current expiration date (ISO 8601)")]
    public required string ExpirationDate { get; set; }
}

[Description("Renew a domain")]
public sealed class RenewCommand : SpaceshipCommand<RenewSettings>
{
    protected override async Task<object> ExecuteAsync(SpaceshipApiClient client, RenewSettings settings)
    {
        var result = await client.PostAsync($"/domains/{settings.Domain}/renew", new
        {
            years = settings.Years,
            currentExpirationDate = settings.ExpirationDate
        });
        return ToObject(result);
    }
}
