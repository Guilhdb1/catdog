namespace CatDog.Api.Modules.Authentication.Services;

public interface IEmailService
{
    Task SendConfirmationEmailAsync(string email, string name, string confirmationUrl, CancellationToken cancellationToken = default);
    Task SendPasswordResetEmailAsync(string email, string name, string resetUrl, CancellationToken cancellationToken = default);
}
