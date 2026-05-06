using AuctionAPI.Context.Models;

namespace AuctionAPI.Context.Repositories.Bid;

public interface IBidRepository
{
    Task<BidDb> CreateBidAsync(BidDb bidDb);
    Task<IEnumerable<BidDb>> GetBidsAsync();
    Task<BidDb> GetBidByIdAsync(Guid bGuid);
    Task SaveChangesAsync();
}