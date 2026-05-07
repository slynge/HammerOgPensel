using System.Text;
using AuctionAPI.Context.Models;
using AuctionAPI.Context.Repositories.Outbox;
using RabbitMQ.Client;

namespace AuctionAPI.Services.Outbox;

internal class OutboxService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IConnection _connection;
    public OutboxService(IServiceScopeFactory scopeFactory, IConnection connection)
    {
        _scopeFactory = scopeFactory;
        _connection = connection;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await using var channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _scopeFactory.CreateScope();
            var outboxRepository = scope.ServiceProvider
                .GetRequiredService<IOutboxRepository>();
            var nonProcessedOutboxMessages = await outboxRepository.GetNonProcessedOutboxMessagesAsync();
            foreach (var nonProcessedOutboxMessage in nonProcessedOutboxMessages)
            {
                var routingKey = DetermineRoutingKey(nonProcessedOutboxMessage.EventType);
                await channel.QueueDeclareAsync(
                    queue: routingKey,
                    durable: true,
                    exclusive: false, 
                    cancellationToken: stoppingToken);
                var body = Encoding.UTF8.GetBytes(nonProcessedOutboxMessage.Payload);
                
                await channel.BasicPublishAsync(
                    exchange: string.Empty,
                    routingKey: routingKey,
                    mandatory: false,
                    body: body, 
                    cancellationToken: stoppingToken);
                nonProcessedOutboxMessage.IsProcessed = true;
                await outboxRepository.SaveChangesAsync();
            }
            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }
    }

    private string DetermineRoutingKey(EventType eventType)
    {
        return eventType switch
        {
            EventType.AuctionClosed => "auction.closed",
            _ => ""
        };
    }
}