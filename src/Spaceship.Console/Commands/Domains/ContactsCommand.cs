using System.ComponentModel;
using Spaceship.Console.Infrastructure;
using Spectre.Console.Cli;

namespace Spaceship.Console.Commands.Domains;

public sealed class ContactsSettings : GlobalSettings
{
    [CommandArgument(0, "<domain>")]
    [Description("Domain name")]
    public required string Domain { get; set; }

    [CommandOption("--registrant <ID>")]
    [Description("Registrant contact ID")]
    public required string Registrant { get; set; }

    [CommandOption("--admin <ID>")]
    [Description("Admin contact ID")]
    public string? Admin { get; set; }

    [CommandOption("--tech <ID>")]
    [Description("Tech contact ID")]
    public string? Tech { get; set; }

    [CommandOption("--billing <ID>")]
    [Description("Billing contact ID")]
    public string? Billing { get; set; }
}

[Description("Update domain contacts")]
public sealed class ContactsCommand : SpaceshipCommand<ContactsSettings>
{
    protected override async Task<object> ExecuteAsync(SpaceshipApiClient client, ContactsSettings settings)
    {
        var body = new Dictionary<string, object?>
        {
            ["registrant"] = settings.Registrant,
            ["admin"] = settings.Admin,
            ["tech"] = settings.Tech,
            ["billing"] = settings.Billing
        };

        var result = await client.PutAsync($"/domains/{settings.Domain}/contacts", body);
        return ToObject(result);
    }
}
