using System.ComponentModel;
using Spaceship.Console.Infrastructure;
using Spectre.Console.Cli;

namespace Spaceship.Console.Commands.Domains;

public sealed class GetSettings : GlobalSettings
{
    [CommandArgument(0, "<domain>")]
    [Description("Domain name (ASCII format)")]
    public required string Domain { get; set; }
}

[Description("Get domain details")]
public sealed class GetCommand : SpaceshipCommand<GetSettings>
{
    protected override async Task<object> ExecuteAsync(SpaceshipApiClient client, GetSettings settings)
    {
        var result = await client.GetAsync($"/domains/{settings.Domain}");
        return ToObject(result);
    }
}
