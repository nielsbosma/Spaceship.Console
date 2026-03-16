using System.ComponentModel;
using Spaceship.Console.Infrastructure;
using Spectre.Console.Cli;

namespace Spaceship.Console.Commands.Domains;

public sealed class PrivacySettings : GlobalSettings
{
    [CommandArgument(0, "<domain>")]
    [Description("Domain name")]
    public required string Domain { get; set; }

    [CommandOption("--level <LEVEL>")]
    [Description("Privacy level: public or high")]
    public required string Level { get; set; }
}

[Description("Update domain privacy preference")]
public sealed class PrivacyCommand : SpaceshipCommand<PrivacySettings>
{
    private static readonly HashSet<string> ValidLevels = new(StringComparer.OrdinalIgnoreCase) { "public", "high" };

    protected override async Task<object> ExecuteAsync(SpaceshipApiClient client, PrivacySettings settings)
    {
        if (!ValidLevels.Contains(settings.Level))
            throw new SpaceshipException("Invalid privacy level. Must be 'public' or 'high'.");

        var result = await client.PutAsync($"/domains/{settings.Domain}/privacy/preference", new
        {
            privacyLevel = settings.Level,
            userConsent = true
        });
        return ToObject(result);
    }
}
