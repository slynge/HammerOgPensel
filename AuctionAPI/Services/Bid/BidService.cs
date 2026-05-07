using System.Text.Json;
using AuctionAPI.Context.Mappers;
using AuctionAPI.Context.Models;
using AuctionAPI.Context.Repositories.Bid;
using AuctionAPI.Context.Repositories.Job;
using AuctionAPI.Context.Repositories.Outbox;
using AuctionAPI.Services.Exceptions;
using DTO.Bid;

namespace AuctionAPI.Services.Bid;

internal class BidService : IBidService
{
    private readonly IBidRepository _bidRepository;
    private readonly IJobRepository _jobRepository;
    private readonly IOutboxRepository _outboxRepository;

    public BidService(IBidRepository bidRepository, IJobRepository jobRepository, IOutboxRepository outboxRepository)
    {
        _bidRepository = bidRepository;
        _jobRepository = jobRepository;
        _outboxRepository = outboxRepository;
    }

    public async Task<BidDto> CreateBidAsync(BidDto bidDto)
    {
        if (bidDto.Price < 0)
        {
            throw new ArgumentException(
                $"Bid price cannot be negative. Received: {bidDto.Price}", 
                nameof(bidDto.Price)
            );
        }
        
        if(!Guid.TryParse(bidDto.JobId, out var jGuid))
        {
            throw new ArgumentException(
                $"'{bidDto.JobId}' is not a valid GUID for jobId.", 
                nameof(bidDto.JobId)
            );
        }

        var jobDb = await _jobRepository.GetJobByIdWithBidsAsync(jGuid);
        
        if (!jobDb.Open)
        {
            throw new JobClosedException(bidDto.JobId);
        }
        var createdBidDb = await _bidRepository.CreateBidAsync(BidMapper.ToEntity(bidDto));

        if (jobDb.Bids.Count == 3)
        {
            jobDb.Open = false;
            await _outboxRepository.CreateOutboxMessageAsync(new OutboxMessageDb
            {
                EventType = EventType.AuctionClosed,
                Payload = JsonSerializer.Serialize(JobMapper.ToDto(jobDb))
            });
        }
        
        await _bidRepository.SaveChangesAsync();
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