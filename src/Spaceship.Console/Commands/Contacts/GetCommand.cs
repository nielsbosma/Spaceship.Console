using System.ComponentModel;
using Spaceship.Console.Infrastructure;
using Spectre.Console.Cli;

namespace Spaceship.Console.Commands.Contacts;

public sealed class GetSettings : GlobalSettings
{
    [CommandArgument(0, "<id>")]
    [Description("Contact ID")]
    public required string Id { get; set; }
}

[Description("Get contact details")]
public sealed class GetCommand : SpaceshipCommand<GetSettings>
{
    protected override async Task<object> ExecuteAsync(SpaceshipApiClient client, GetSettings settings)
    {
        var result = await client.GetAsync($"/contacts/{settings.Id}");
        return ToObject(result);
    }
}
