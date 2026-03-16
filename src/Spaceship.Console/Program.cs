using Spectre.Console.Cli;

var app = new CommandApp();

app.Configure(config =>
{
    config.SetApplicationName("spaceship");
    config.SetApplicationVersion("1.0.0");

    config.AddBranch("domains", domains =>
    {
        domains.SetDescription("Domain management");
        domains.AddCommand<Spaceship.Console.Commands.Domains.ListCommand>("list");
        domains.AddCommand<Spaceship.Console.Commands.Domains.GetCommand>("get");
        domains.AddCommand<Spaceship.Console.Commands.Domains.CheckCommand>("check");
        domains.AddCommand<Spaceship.Console.Commands.Domains.CheckBatchCommand>("check-batch");
        domains.AddCommand<Spaceship.Console.Commands.Domains.RegisterCommand>("register");
        domains.AddCommand<Spaceship.Console.Commands.Domains.DeleteCommand>("delete");
        domains.AddCommand<Spaceship.Console.Commands.Domains.RenewCommand>("renew");
        domains.AddCommand<Spaceship.Console.Commands.Domains.RestoreCommand>("restore");
        domains.AddCommand<Spaceship.Console.Commands.Domains.AutorenewCommand>("autorenew");
        domains.AddCommand<Spaceship.Console.Commands.Domains.NameserversCommand>("nameservers");
        domains.AddCommand<Spaceship.Console.Commands.Domains.ContactsCommand>("contacts");
        domains.AddCommand<Spaceship.Console.Commands.Domains.PrivacyCommand>("privacy");
        domains.AddCommand<Spaceship.Console.Commands.Domains.AuthCodeCommand>("auth-code");
        domains.AddCommand<Spaceship.Console.Commands.Domains.TransferCommand>("transfer");
        domains.AddCommand<Spaceship.Console.Commands.Domains.TransferStatusCommand>("transfer-status");
        domains.AddCommand<Spaceship.Console.Commands.Domains.TransferLockCommand>("transfer-lock");
    });

    config.AddBranch("dns", dns =>
    {
        dns.SetDescription("DNS record management");
        dns.AddCommand<Spaceship.Console.Commands.Dns.ListCommand>("list");
        dns.AddCommand<Spaceship.Console.Commands.Dns.SaveCommand>("save");
        dns.AddCommand<Spaceship.Console.Commands.Dns.DeleteCommand>("delete");
    });

    config.AddBranch("contacts", contacts =>
    {
        contacts.SetDescription("Contact management");
        contacts.AddCommand<Spaceship.Console.Commands.Contacts.SaveCommand>("save");
        contacts.AddCommand<Spaceship.Console.Commands.Contacts.GetCommand>("get");
    });

    config.AddBranch("sellerhub", sellerhub =>
    {
        sellerhub.SetDescription("SellerHub management");
        sellerhub.AddCommand<Spaceship.Console.Commands.SellerHub.ListCommand>("list");
        sellerhub.AddCommand<Spaceship.Console.Commands.SellerHub.GetCommand>("get");
        sellerhub.AddCommand<Spaceship.Console.Commands.SellerHub.CreateCommand>("create");
        sellerhub.AddCommand<Spaceship.Console.Commands.SellerHub.UpdateCommand>("update");
        sellerhub.AddCommand<Spaceship.Console.Commands.SellerHub.DeleteCommand>("delete");
        sellerhub.AddCommand<Spaceship.Console.Commands.SellerHub.CheckoutCommand>("checkout");
        sellerhub.AddCommand<Spaceship.Console.Commands.SellerHub.VerificationCommand>("verification");
    });

    config.AddBranch("operations", operations =>
    {
        operations.SetDescription("Async operation tracking");
        operations.AddCommand<Spaceship.Console.Commands.Operations.GetCommand>("get");
    });
});

return app.Run(args);
