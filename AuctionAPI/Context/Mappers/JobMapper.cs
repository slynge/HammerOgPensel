using AuctionAPI.Context.Models;
using DTO.Job;

namespace AuctionAPI.Context.Mappers;

internal class JobMapper
{
    public static JobDb ToEntity(JobDto jobDto)
    {
        return new JobDb
        {
            Id = Guid.NewGuid(),
            Description = jobDto.Description,
            Email = jobDto.Email,
            Zip = jobDto.Zip,
            Category = jobDto.Category
        };
    }

    public static JobDto ToDto(JobDb jobDb)
    {
        return new JobDto(
            Id: jobDb.Id.ToString(),
            Description: jobDb.Description,
            Email: jobDb.Email,
            Zip: jobDb.Zip,
            Category: jobDb.Category,
            Open: jobDb.Open,
            Bids: BidMapper.ToDtos(jobDb.Bids)
        );
    }
}