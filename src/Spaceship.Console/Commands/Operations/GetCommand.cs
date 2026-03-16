using System.ComponentModel;
using Spaceship.Console.Infrastructure;
using Spectre.Console.Cli;

namespace Spaceship.Console.Commands.Operations;

public sealed class GetSettings : GlobalSettings
{
    [CommandArgument(0, "<id>")]
    [Description("Async operation ID")]
    public required string Id { get; set; }
}

[Description("Get async operation status")]
public sealed class GetCommand : SpaceshipCommand<GetSettings>
{
    protected override async Task<object> ExecuteAsync(SpaceshipApiClient client, GetSettings settings)
    {
        var result = await client.GetAsync($"/async-operations/{settings.Id}");
        return ToObject(result);
    }
}
