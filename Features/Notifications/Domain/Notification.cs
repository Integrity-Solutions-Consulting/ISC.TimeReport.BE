namespace isc_tmr_backend.Features.Notifications.Domain;

public class Notification
{
    public Guid Id { get; private set; }
    public string To { get; private set; }
    public string Subject { get; private set; }
    public string Body { get; private set; }
    public NotificationChannel Channel { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private Notification()
    {
        To = string.Empty;
        Subject = string.Empty;
        Body = string.Empty;
    }

    public static Notification Create(
        string to,
        string subject,
        string body,
        NotificationChannel channel)
    {
        return new Notification
        {
            Id = Guid.NewGuid(),
            To = to,
            Subject = subject,
            Body = body,
            Channel = channel,
            CreatedAt = DateTime.UtcNow
        };
    }
}