using System.ComponentModel;
using Spaceship.Console.Infrastructure;
using Spectre.Console.Cli;

namespace Spaceship.Console.Commands.Domains;

public sealed class RegisterSettings : GlobalSettings
{
    [CommandArgument(0, "<domain>")]
    [Description("Domain name to register")]
    public required string Domain { get; set; }

    [CommandOption("--years <YEARS>")]
    [Description("Registration period (1-10)")]
    [DefaultValue(1)]
    public int Years { get; set; } = 1;

    [CommandOption("--auto-renew")]
    [Description("Enable auto-renewal")]
    public bool AutoRenew { get; set; }

    [CommandOption("--privacy <LEVEL>")]
    [Description("Privacy level: public or high")]
    [DefaultValue("high")]
    public string Privacy { get; set; } = "high";

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

[Description("Register a domain")]
public sealed class RegisterCommand : SpaceshipCommand<RegisterSettings>
{
    protected override async Task<object> ExecuteAsync(SpaceshipApiClient client, RegisterSettings settings)
    {
        var body = new Dictionary<string, object>
        {
            ["autoRenew"] = settings.AutoRenew,
            ["years"] = settings.Years,
            ["privacyProtection"] = new Dictionary<string, object>
            {
                ["level"] = settings.Privacy,
                ["userConsent"] = true
            },
            ["contacts"] = new Dictionary<string, object?>
            {
                ["registrant"] = settings.Registrant,
                ["admin"] = settings.Admin,
                ["tech"] = settings.Tech,
                ["billing"] = settings.Billing
            }
        };

        var result = await client.PostAsync($"/domains/{settings.Domain}", body);
        return ToObject(result);
    }
}
