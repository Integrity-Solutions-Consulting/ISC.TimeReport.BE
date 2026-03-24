namespace isc_tmr_backend.Features.Notifications.Infrastructure;

using System.Text.Json;

public interface ISqsPublisher
{
    Task PublishAsync<T>(T message, string queueUrl);
}

public class SqsPublisher(IConfiguration config) : ISqsPublisher
{
    public async Task PublishAsync<T>(T message, string queueUrl)
    {
        // TODO: reemplazar con IAmazonSQS del SDK de AWS
        var json = JsonSerializer.Serialize(message);
        await Task.CompletedTask;
        Console.WriteLine($"[Mock SQS Publisher] {config}");
        Console.WriteLine($"[Mock SQS Publisher] Message published:");
        Console.WriteLine($"[SQS] → {queueUrl} | {json}");
    }
}