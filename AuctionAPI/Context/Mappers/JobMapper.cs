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
            Zip = jobDto.Zip,
            Category = jobDto.Category
        };
    }

    public static JobDto ToDto(JobDb jobDb)
    {
        return new JobDto(
            Id: jobDb.Id.ToString(),
            Description: jobDb.Description,
            Zip: jobDb.Zip,
            Category: jobDb.Category,
            Status: jobDb.Status,
            Bids: BidMapper.ToDtos(jobDb.Bids)
        );
    }
}