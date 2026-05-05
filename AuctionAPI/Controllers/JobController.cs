using AuctionAPI.Services.Job;
using DTO.Job;
using Microsoft.AspNetCore.Mvc;

namespace AuctionAPI.Controllers;

[ApiController]
[Route("api/jobs")]
public class JobController : ControllerBase
{
    private readonly IJobService _jobService;
    public JobController(IJobService jobService)
    {
        _jobService = jobService;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateJobAsync([FromBody] JobDto jobDto)
    {
        var createdJobDto = await _jobService.CreateJobAsync(jobDto);
        return CreatedAtAction(nameof(GetJobByIdAsync), new { id = createdJobDto.Id}, createdJobDto);
    }

    [HttpGet]
    public async Task<IActionResult> GetJobsAsync()
    {
        return Ok(await _jobService.GetJobsAsync());
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetJobByIdAsync(string id)
    {
        try
        {
            return Ok(await _jobService.GetJobByIdAsync(id));
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
}