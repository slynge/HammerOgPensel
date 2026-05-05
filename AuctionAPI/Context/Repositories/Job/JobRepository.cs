using AuctionAPI.Context.Models;
using Microsoft.EntityFrameworkCore;

namespace AuctionAPI.Context.Repositories.Job;

internal class JobRepository : IJobRepository
{
    private readonly AuctionContext _context;

    public JobRepository(AuctionContext context)
    {
        _context = context;
    }
    
    public async Task<JobDb> CreateJobAsync(JobDb jobDb)
    {
        await _context.Jobs.AddAsync(jobDb);
        await _context.SaveChangesAsync();
        return jobDb;
    } 

    public async Task<IEnumerable<JobDb>> GetJobsAsync()
    {
        return await _context.Jobs.ToListAsync();
    }

    public async Task<JobDb> GetJobByIdAsync(Guid jGuid)
    {
        var foundJobDb = await _context.Jobs.FindAsync(jGuid);
        if (foundJobDb is null)
        {
            throw new KeyNotFoundException($"Job with id '{jGuid}' was not found.");
        }

        return foundJobDb;
    }
}