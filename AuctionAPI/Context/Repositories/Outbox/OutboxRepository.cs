using AuctionAPI.Context.Models;
using Microsoft.EntityFrameworkCore;

namespace AuctionAPI.Context.Repositories.Outbox;

internal class OutboxRepository : IOutboxRepository
{
    private readonly AuctionContext _context;
    public OutboxRepository(AuctionContext context)
    {
        _context = context;
    }

    public async Task CreateOutboxMessageAsync(OutboxMessageDb outboxMessageDb)
    {
        await _context.OutboxMessages.AddAsync(outboxMessageDb);
    }

    public async Task<IEnumerable<OutboxMessageDb>> GetNonProcessedOutboxMessagesAsync()
    {
        return await _context.OutboxMessages.Where(o => !o.IsProcessed).ToListAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}