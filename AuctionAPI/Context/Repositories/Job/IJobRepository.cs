using AuctionAPI.Context.Models;

namespace AuctionAPI.Context.Repositories.Job;

public interface IJobRepository
{
    Task<JobDb> CreateJobAsync(JobDb jobDb);
    Task<IEnumerable<JobDb>> GetJobsAsync();
    Task<JobDb> GetJobByIdAsync(Guid jGuid);
    Task<bool> IsJobOpenAsync(Guid jGuid);
    Task SaveChangesAsync();
}