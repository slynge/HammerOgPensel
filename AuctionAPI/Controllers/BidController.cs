using AuctionAPI.Services.Bid;
using AuctionAPI.Services.Exceptions;
using DTO.Bid;
using Microsoft.AspNetCore.Mvc;

namespace AuctionAPI.Controllers;

[ApiController]
[Route("/api/bids")]
public class BidController : ControllerBase
{
    private readonly IBidService _bidService;
    public BidController(IBidService bidService)
    {
        _bidService = bidService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateBidAsync([FromBody] BidDto bidDto)
    {
        try
        {
            var createdBidDto = await _bidService.CreateBidAsync(bidDto);
            return AcceptedAtAction(nameof(GetBidByIdAsync), new { id = createdBidDto.Id }, createdBidDto);
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (JobClosedException e)
        {
            return Conflict(e.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetBidsAsync()
    {
        return Ok(await _bidService.GetBidsAsync());
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetBidByIdAsync([FromRoute] string id)
    {
        try
        {
            return Ok(await _bidService.GetBidByIdAsync(id));
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