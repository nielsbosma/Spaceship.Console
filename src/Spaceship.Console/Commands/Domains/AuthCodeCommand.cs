using System.ComponentModel;
using Spaceship.Console.Infrastructure;
using Spectre.Console.Cli;

namespace Spaceship.Console.Commands.Domains;

public sealed class AuthCodeSettings : GlobalSettings
{
    [CommandArgument(0, "<domain>")]
    [Description("Domain name")]
    public required string Domain { get; set; }
}

[Description("Get domain transfer auth code")]
public sealed class AuthCodeCommand : SpaceshipCommand<AuthCodeSettings>
{
    protected override async Task<object> ExecuteAsync(SpaceshipApiClient client, AuthCodeSettings settings)
    {
        var result = await client.GetAsync($"/domains/{settings.Domain}/transfer/auth-code");
        return ToObject(result);
    }
}
