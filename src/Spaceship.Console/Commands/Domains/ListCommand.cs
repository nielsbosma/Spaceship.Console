using System.ComponentModel;
using Spaceship.Console.Infrastructure;
using Spectre.Console.Cli;

namespace Spaceship.Console.Commands.Domains;

public sealed class ListSettings : PaginatedSettings
{
    [CommandOption("--order-by")]
    [Description("Sort by: name, unicodeName, registrationDate, expirationDate")]
    public string? OrderBy { get; set; }
}

[Description("List all domains")]
public sealed class ListCommand : SpaceshipCommand<ListSettings>
{
    protected override async Task<object> ExecuteAsync(SpaceshipApiClient client, ListSettings settings)
    {
        var query = $"/domains?take={settings.Take}&skip={settings.Skip}";
        if (!string.IsNullOrWhiteSpace(settings.OrderBy))
            query += $"&orderBy={settings.OrderBy}";

        var result = await client.GetAsync(query);
        return ToObject(result);
    }
}
