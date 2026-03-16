using System.ComponentModel;
using Spaceship.Console.Infrastructure;
using Spectre.Console.Cli;

namespace Spaceship.Console.Commands.Domains;

public sealed class NameserversSettings : GlobalSettings
{
    [CommandArgument(0, "<domain>")]
    [Description("Domain name")]
    public required string Domain { get; set; }

    [CommandOption("--provider <PROVIDER>")]
    [Description("Nameserver provider: basic or custom")]
    public required string Provider { get; set; }

    [CommandOption("--hosts <HOSTS>")]
    [Description("Comma-separated nameserver hosts (required for custom, 2-12)")]
    public string? Hosts { get; set; }
}

[Description("Update domain nameservers")]
public sealed class NameserversCommand : SpaceshipCommand<NameserversSettings>
{
    protected override async Task<object> ExecuteAsync(SpaceshipApiClient client, NameserversSettings settings)
    {
        var body = new Dictionary<string, object>
        {
            ["provider"] = settings.Provider
        };

        if (settings.Provider.Equals("custom", StringComparison.OrdinalIgnoreCase))
        {
            if (string.IsNullOrWhiteSpace(settings.Hosts))
                throw new SpaceshipException("--hosts is required when provider is custom.");

            var hosts = settings.Hosts.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            if (hosts.Length < 2 || hosts.Length > 12)
                throw new SpaceshipException("Custom nameservers require 2-12 hosts.");

            body["hosts"] = hosts;
        }

        var result = await client.PutAsync($"/domains/{settings.Domain}/nameservers", body);
        return ToObject(result);
    }
}
