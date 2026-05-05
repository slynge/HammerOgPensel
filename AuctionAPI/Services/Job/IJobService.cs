using DTO.Job;

namespace AuctionAPI.Services.Job;

internal interface IJobService
{
    Task<JobDto> CreateJobAsync(JobDto jobDto);
    Task<IEnumerable<JobDto>> GetJobsAsync();
    Task<JobDto> GetJobByIdAsync(string id);
}