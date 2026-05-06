using AuctionAPI.Context.Models;
using Microsoft.EntityFrameworkCore;

namespace AuctionAPI.Context.Repositories.Bid;

internal class BidRepository : IBidRepository
{
    private readonly AuctionContext _context;
    public BidRepository(AuctionContext context)
    {
        _context = context;
    }

    public async Task<BidDb> CreateBidAsync(BidDb bidDb)
    {
        await _context.Bids.AddAsync(bidDb);
        return bidDb;
    }
    
    public async Task<IEnumerable<BidDb>> GetBidsAsync()
    {
        return await _context.Bids.ToListAsync();
    }

    public async Task<BidDb> GetBidByIdAsync(Guid bGuid)
    {
        var foundBidDb = await _context.Bids.FindAsync(bGuid);
        if (foundBidDb is null)
        {
            throw new KeyNotFoundException($"Bid with id '{bGuid}' was not found.");
        }

        return foundBidDb;
    }
    
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}