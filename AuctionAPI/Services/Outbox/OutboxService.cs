using System.Text;
using System.Text.Json;
using AuctionAPI.Context.Repositories.Job;
using AuctionAPI.Context.Repositories.Outbox;
using DTO.Job;
using Microsoft.OpenApi;
using RabbitMQ.Client;

namespace AuctionAPI.Services.Outbox;

internal class OutboxService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IConnection _connection;
    private readonly IChannel _channel;
    public OutboxService(IServiceScopeFactory scopeFactory, IConnection connection)
    {
        _scopeFactory = scopeFactory;
        _connection = connection;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await using var _channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _scopeFactory.CreateScope();
            var outboxRepository = scope.ServiceProvider
                .GetRequiredService<IOutboxRepository>();
            var nonProcessedOutboxMessages = await outboxRepository.GetNonProcessedOutboxMessagesAsync();
            foreach (var nonProcessedOutboxMessage in nonProcessedOutboxMessages)
            {
                var body = Encoding.UTF8.GetBytes(nonProcessedOutboxMessage.Payload);

                await _channel.BasicPublishAsync(
                    exchange: string.Empty,
                    routingKey: "",
                    mandatory: false,
                    body: body, 
                    cancellationToken: stoppingToken);
                nonProcessedOutboxMessage.IsProcessed = true;
                await outboxRepository.SaveChangesAsync();
            }
            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }
    }
}