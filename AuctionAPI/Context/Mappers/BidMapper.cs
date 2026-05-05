using AuctionAPI.Context.Models;
using DTO.Bid;

namespace AuctionAPI.Context.Mappers;

internal class BidMapper
{
    public static BidDb ToEntity(BidDto bidDto)
    {
        return new BidDb
        {
            Id = Guid.NewGuid(),
            Price = bidDto.Price,
            Name = bidDto.Name,
            Email = bidDto.Email,
            JobId = Guid.Parse(bidDto.JobId)
        };
    }
    public static BidDto ToDto(BidDb bidDb)
    {
        return new BidDto(
            Price: bidDb.Price,
            Name: bidDb.Name, 
            Email: bidDb.Email,
            JobId: bidDb.JobId.ToString()
        );
    }
    
    public static IEnumerable<BidDto> ToDtos(IEnumerable<BidDb> bidDbs)
    {
        return bidDbs.Select(ToDto);
    }
}