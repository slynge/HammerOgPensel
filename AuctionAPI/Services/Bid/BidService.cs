using AuctionAPI.Context.Mappers;
using AuctionAPI.Context.Repositories.Bid;
using DTO.Bid;

namespace AuctionAPI.Services.Bid;

internal class BidService : IBidService
{
    private readonly IBidRepository _bidRepository;
    public BidService(IBidRepository bidRepository)
    {
        _bidRepository = bidRepository;
    }

    public async Task<BidDto> CreateBidAsync(BidDto bidDto)
    {
        var createdBidDb = await _bidRepository.CreateBidAsync(BidMapper.ToEntity(bidDto));
        return BidMapper.ToDto(createdBidDb);
    }

    public async Task<IEnumerable<BidDto>> GetBidsAsync()
    {
        var bidDbs = await _bidRepository.GetBidsAsync();
        return bidDbs.Select(BidMapper.ToDto);
    }

    public async Task<BidDto> GetBidByIdAsync(string id)
    {
        if(!Guid.TryParse(id, out var bGuid))
        {
            throw new ArgumentException(
                $"'{id}' is not a valid GUID for bidId.", 
                nameof(id)
            );
        }

        var foundBidDb = await _bidRepository.GetBidByIdAsync(bGuid);
        return BidMapper.ToDto(foundBidDb);
    }
}