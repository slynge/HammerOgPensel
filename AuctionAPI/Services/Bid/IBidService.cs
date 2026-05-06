using DTO.Bid;

namespace AuctionAPI.Services.Bid;

public interface IBidService
{
    Task<BidDto> CreateBidAsync(BidDto bidDto);
    Task<IEnumerable<BidDto>> GetBidsAsync();
    Task<BidDto> GetBidByIdAsync(string id);
}