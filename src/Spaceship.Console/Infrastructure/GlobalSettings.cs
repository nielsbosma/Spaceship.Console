using System.ComponentModel;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Spaceship.Console.Infrastructure;

public class GlobalSettings : CommandSettings
{
    [CommandOption("--api-key")]
    [Description("Spaceship API key (overrides SPACESHIP_API_KEY env var)")]
    public string? ApiKey { get; set; }

    [CommandOption("--api-secret")]
    [Description("Spaceship API secret (overrides SPACESHIP_API_SECRET env var)")]
    public string? ApiSecret { get; set; }

    private static readonly HashSet<string> ValidFormats = new(StringComparer.OrdinalIgnoreCase) { "yaml", "json", "table" };

    [CommandOption("--format")]
    [Description("Output format: yaml, json, or table")]
    [DefaultValue("yaml")]
    public string Format { get; set; } = "yaml";

    public override ValidationResult Validate()
    {
        if (!ValidFormats.Contains(Format))
            return ValidationResult.Error($"Invalid format '{Format}'. Must be one of: yaml, json, table");
        return base.Validate();
    }

    [CommandOption("--no-color")]
    [Description("Disable colored output")]
    public bool NoColor { get; set; }

    [CommandOption("--verbose")]
    [Description("Print HTTP method, URL, and status code to stderr")]
    public bool Verbose { get; set; }

    public (string key, string secret) ResolveCredentials()
    {
        var key = ApiKey;
        if (string.IsNullOrWhiteSpace(key))
            key = Environment.GetEnvironmentVariable("SPACESHIP_API_KEY");
        if (string.IsNullOrWhiteSpace(key))
            throw new SpaceshipException("API key not set. Provide --api-key or set the SPACESHIP_API_KEY environment variable.");

        var secret = ApiSecret;
        if (string.IsNullOrWhiteSpace(secret))
            secret = Environment.GetEnvironmentVariable("SPACESHIP_API_SECRET");
        if (string.IsNullOrWhiteSpace(secret))
            throw new SpaceshipException("API secret not set. Provide --api-secret or set the SPACESHIP_API_SECRET environment variable.");

        return (key, secret);
    }
}

public class PaginatedSettings : GlobalSettings
{
    [CommandOption("--take")]
    [Description("Number of items to return (1-100)")]
    [DefaultValue(20)]
    public int Take { get; set; } = 20;

    [CommandOption("--skip")]
    [Description("Number of items to skip")]
    [DefaultValue(0)]
    public int Skip { get; set; }
}
