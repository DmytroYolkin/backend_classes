namespace labo_01_parking_api.Services;

public record EmailMessage(string To, string Subject, string Body);

public interface IEmailService
{
    Task SendEmailAsync(EmailMessage message, CancellationToken ct = default);
}

public class EmailService : IEmailService
{
    public Task SendEmailAsync(EmailMessage message, CancellationToken ct = default)
    {
        return Task.CompletedTask;
    }
}
