using System.ComponentModel;
using Spaceship.Console.Infrastructure;
using Spectre.Console.Cli;

namespace Spaceship.Console.Commands.Domains;

public sealed class TransferSettings : GlobalSettings
{
    [CommandArgument(0, "<domain>")]
    [Description("Domain name to transfer")]
    public required string Domain { get; set; }

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

    [CommandOption("--auth-code <CODE>")]
    [Description("Domain transfer authorization code")]
    public string? AuthCode { get; set; }
}

[Description("Request domain transfer")]
public sealed class TransferCommand : SpaceshipCommand<TransferSettings>
{
    protected override async Task<object> ExecuteAsync(SpaceshipApiClient client, TransferSettings settings)
    {
        var body = new Dictionary<string, object>
        {
            ["autoRenew"] = settings.AutoRenew,
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

        if (!string.IsNullOrWhiteSpace(settings.AuthCode))
            body["authCode"] = settings.AuthCode;

        var result = await client.PostAsync($"/domains/{settings.Domain}/transfer", body);
        return ToObject(result);
    }
}
