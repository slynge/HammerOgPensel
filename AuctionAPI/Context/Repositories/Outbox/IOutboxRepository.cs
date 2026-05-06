using AuctionAPI.Context.Models;

namespace AuctionAPI.Context.Repositories.Outbox;

public interface IOutboxRepository
{
    Task CreateOutboxMessageAsync(OutboxMessageDb outboxMessageDb);
    Task<IEnumerable<OutboxMessageDb>> GetNonProcessedOutboxMessagesAsync();

    Task SaveChangesAsync();
}