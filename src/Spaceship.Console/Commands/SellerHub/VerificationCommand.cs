using System.ComponentModel;
using Spaceship.Console.Infrastructure;
using Spectre.Console.Cli;

namespace Spaceship.Console.Commands.SellerHub;

public sealed class VerificationSettings : GlobalSettings
{
    [CommandArgument(0, "<id>")]
    [Description("SellerHub domain ID")]
    public required string Id { get; set; }
}

[Description("Get SellerHub verification records")]
public sealed class VerificationCommand : SpaceshipCommand<VerificationSettings>
{
    protected override async Task<object> ExecuteAsync(SpaceshipApiClient client, VerificationSettings settings)
    {
        var result = await client.GetAsync($"/sellerhub/domains/{settings.Id}/verification");
        return ToObject(result);
    }
}
