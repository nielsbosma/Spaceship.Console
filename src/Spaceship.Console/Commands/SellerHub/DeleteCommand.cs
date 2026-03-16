using System.ComponentModel;
using Spaceship.Console.Infrastructure;
using Spectre.Console.Cli;

namespace Spaceship.Console.Commands.SellerHub;

public sealed class DeleteSettings : GlobalSettings
{
    [CommandArgument(0, "<id>")]
    [Description("SellerHub domain ID")]
    public required string Id { get; set; }
}

[Description("Delete SellerHub domain")]
public sealed class DeleteCommand : SpaceshipCommand<DeleteSettings>
{
    protected override async Task<object> ExecuteAsync(SpaceshipApiClient client, DeleteSettings settings)
    {
        var result = await client.DeleteAsync($"/sellerhub/domains/{settings.Id}");
        return ToObject(result);
    }
}
