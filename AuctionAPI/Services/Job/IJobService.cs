using DTO.Job;

namespace AuctionAPI.Services.Job;

public interface IJobService
{
    Task<JobDto> CreateJobAsync(JobDto jobDto);
    Task<IEnumerable<JobDto>> GetJobsAsync();
    Task<JobDto> GetJobByIdAsync(string id);
    Task<JobDto> GetJobByIdWithBidsAsync(string id);
}