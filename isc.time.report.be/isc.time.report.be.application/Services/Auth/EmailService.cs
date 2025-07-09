using isc.time.report.be.domain.Entity.Emails;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace isc.time.report.be.application.Services.Auth
{
    public class EmailService
    {
        private readonly EmailSettings _settings;

        public EmailService(IOptions<EmailSettings> options)
        {
            _settings = options.Value;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            Console.WriteLine("[SMTP DEBUG] ----------");
            Console.WriteLine($"Host: {_settings.SmtpServer}");
            Console.WriteLine($"Port: {_settings.Port}");
            Console.WriteLine($"Username: {_settings.Username}");
            Console.WriteLine($"Sender: {_settings.SenderName} <{_settings.SenderEmail}>");
            Console.WriteLine("[SMTP DEBUG] ----------");

            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(_settings.SenderName, _settings.SenderEmail));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;

            var builder = new BodyBuilder
            {
                HtmlBody = body
            };
            email.Body = builder.ToMessageBody();

            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            await smtp.ConnectAsync(_settings.SmtpServer, _settings.Port, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_settings.Username, _settings.Password);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
