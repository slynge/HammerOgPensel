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
        return jobDb;
    } 

    public async Task<IEnumerable<JobDb>> GetJobsAsync()
    {
        return await _context.Jobs.ToListAsync();
    }

    public async Task<JobDb> GetJobByIdAsync(Guid jGuid)
    {
        var foundJobDb = await _context.Jobs.FindAsync(jGuid);
        return foundJobDb ?? throw new KeyNotFoundException($"Job with id '{jGuid}' was not found.");
    }

    public async Task<JobDb> GetJobByIdWithBidsAsync(Guid jGuid)
    {
        var foundJobDb = await _context.Jobs.Include(j => j.Bids).FirstOrDefaultAsync(j => j.Id == jGuid);
        return foundJobDb ?? throw new KeyNotFoundException($"Job with id '{jGuid}' was not found.");
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}