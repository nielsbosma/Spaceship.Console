using System.ComponentModel;
using Spaceship.Console.Infrastructure;
using Spectre.Console.Cli;

namespace Spaceship.Console.Commands.Dns;

public sealed class ListSettings : GlobalSettings
{
    [CommandArgument(0, "<domain>")]
    [Description("Domain name")]
    public required string Domain { get; set; }

    [CommandOption("--type <TYPE>")]
    [Description("Filter by record type (A, AAAA, CNAME, MX, TXT, SRV, NS)")]
    public string? Type { get; set; }

    [CommandOption("--name <NAME>")]
    [Description("Filter by record name")]
    public string? Name { get; set; }
}

[Description("List DNS records")]
public sealed class ListCommand : SpaceshipCommand<ListSettings>
{
    protected override async Task<object> ExecuteAsync(SpaceshipApiClient client, ListSettings settings)
    {
        var query = $"/domains/{settings.Domain}/dns-records";
        var sep = '?';
        if (!string.IsNullOrWhiteSpace(settings.Type))
        {
            query += $"{sep}type={settings.Type}";
            sep = '&';
        }
        if (!string.IsNullOrWhiteSpace(settings.Name))
            query += $"{sep}name={settings.Name}";

        var result = await client.GetAsync(query);
        return ToObject(result);
    }
}
