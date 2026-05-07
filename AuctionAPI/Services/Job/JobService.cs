using AuctionAPI.Context.Mappers;
using AuctionAPI.Context.Repositories.Job;
using AuctionAPI.Context.Repositories.Outbox;
using DTO.Job;

namespace AuctionAPI.Services.Job;

internal class JobService : IJobService
{
    private readonly IJobRepository _jobRepository;
    public JobService(IJobRepository jobRepository, IOutboxRepository outboxRepository)
    {
        _jobRepository = jobRepository;
    }
    public async Task<JobDto> CreateJobAsync(JobDto jobDto)
    {
        var createdJobDb = await _jobRepository.CreateJobAsync(JobMapper.ToEntity(jobDto));
        await _jobRepository.SaveChangesAsync();
        return JobMapper.ToDto(createdJobDb);
    }
    
    public async Task<IEnumerable<JobDto>> GetJobsAsync()
    {
        var jobDbs = await _jobRepository.GetJobsAsync();
        return jobDbs.Select(JobMapper.ToDto);
    }

    public async Task<JobDto> GetJobByIdAsync(string id)
    {
        if(!Guid.TryParse(id, out var jGuid))
        {
            throw new ArgumentException(
                $"'{id}' is not a valid GUID for jobId.", 
                nameof(id)
            );
        }

        var foundJobDb = await _jobRepository.GetJobByIdAsync(jGuid);
        return JobMapper.ToDto(foundJobDb);
    }
    
    public async Task<JobDto> GetJobByIdWithBidsAsync(string id)
    {
        if(!Guid.TryParse(id, out var jGuid))
        {
            throw new ArgumentException(
                $"'{id}' is not a valid GUID for jobId.", 
                nameof(id)
            );
        }

        var foundJobDb = await _jobRepository.GetJobByIdWithBidsAsync(jGuid);
        return JobMapper.ToDto(foundJobDb);
    }
}