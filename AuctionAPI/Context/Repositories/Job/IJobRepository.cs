using AuctionAPI.Context.Models;

namespace AuctionAPI.Context.Repositories.Job;

internal interface IJobRepository
{
    Task<JobDb> CreateJobAsync(JobDb jobDb);
    Task<IEnumerable<JobDb>> GetJobsAsync();
    Task<JobDb> GetJobByIdAsync(Guid jGuid);
}