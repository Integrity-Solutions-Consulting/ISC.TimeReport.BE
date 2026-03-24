namespace isc_tmr_backend.Features.Notifications.Infrastructure;


public interface IEmailSender
{
    Task SendAsync(string to, string subject, string body);
}

public class EmailSender(IConfiguration config) : IEmailSender
{
    public async Task SendAsync(string to, string subject, string body)
    {
        // TODO: reemplazar con SendGrid / AWS SES
        await Task.CompletedTask;
        Console.WriteLine($"[Mock Email Sender] {config}");
        Console.WriteLine($"[Email] → {to} | {subject}");
    }
}