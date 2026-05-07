using System.Text;
using System.Text.Json;
using DTO.Job;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace NotificationService;

public class NotificationWorker : BackgroundService
{
    private readonly IConnection _connection;
    public NotificationWorker(IConnection connection)
    {
        _connection = connection;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await using IChannel channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);
        await channel.QueueDeclareAsync(
            queue: "auction.closed",
            durable: true,
            exclusive: false,
            cancellationToken: stoppingToken);
        
            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (o, ea) =>
            {
                var body = Encoding.UTF8.GetString(ea.Body.ToArray());
                var jobDto = JsonSerializer.Deserialize<JobDto>(body);
                Console.WriteLine(jobDto!.Bids);
            };
            
            await channel.BasicConsumeAsync(
                queue: "auction.closed",
                autoAck: true,
                consumer: consumer, 
                cancellationToken: stoppingToken);
    }
    
}