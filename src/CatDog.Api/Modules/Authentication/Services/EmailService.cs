namespace CatDog.Api.Modules.Authentication.Services;

public class EmailService : IEmailService
{
    public Task SendConfirmationEmailAsync(string email, string name, string confirmationUrl, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public Task SendPasswordResetEmailAsync(string email, string name, string resetUrl, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}
