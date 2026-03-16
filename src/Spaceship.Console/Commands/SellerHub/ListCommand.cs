using System.ComponentModel;
using Spaceship.Console.Infrastructure;
using Spectre.Console.Cli;

namespace Spaceship.Console.Commands.SellerHub;

[Description("List SellerHub domains")]
public sealed class ListCommand : SpaceshipCommand<PaginatedSettings>
{
    protected override async Task<object> ExecuteAsync(SpaceshipApiClient client, PaginatedSettings settings)
    {
        var result = await client.GetAsync($"/sellerhub/domains?take={settings.Take}&skip={settings.Skip}");
        return ToObject(result);
    }
}
