using AuctionAPI.Context.Mappers;
using AuctionAPI.Context.Repositories.Bid;
using AuctionAPI.Context.Repositories.Job;
using AuctionAPI.Services.Exceptions;
using DTO.Bid;

namespace AuctionAPI.Services.Bid;

internal class BidService : IBidService
{
    private readonly IBidRepository _bidRepository;
    private readonly IJobRepository _jobRepository;
    public BidService(IBidRepository bidRepository, IJobRepository jobRepository)
    {
        _bidRepository = bidRepository;
        _jobRepository = jobRepository;
    }

    public async Task<BidDto> CreateBidAsync(BidDto bidDto)
    {
        if (!await IsJobOpenAsync(bidDto.JobId))
        {
            throw new JobClosedException(bidDto.JobId);
        }
        
        if (bidDto.Price < 0)
        {
            throw new ArgumentException(
                $"Bid price cannot be negative. Received: {bidDto.Price}", 
                nameof(bidDto.Price)
            );
        }
        var createdBidDb = await _bidRepository.CreateBidAsync(BidMapper.ToEntity(bidDto));
        await _bidRepository.SaveChangesAsync();
        return BidMapper.ToDto(createdBidDb);
    }

    private async Task<bool> IsJobOpenAsync(string jobId)
    {
        if(!Guid.TryParse(jobId, out var jGuid))
        {
            throw new ArgumentException(
                $"'{jobId}' is not a valid GUID for jobId.", 
                nameof(jobId)
            );
        }

        return await _jobRepository.IsJobOpenAsync(jGuid);
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