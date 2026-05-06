using AuctionAPI.Services.Bid;
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
        var createdBidDto = await _bidService.CreateBidAsync(bidDto);
        return CreatedAtAction(nameof(GetBidByIdAsync), new { id = createdBidDto.Id }, createdBidDto);
    }

    [HttpGet]
    public async Task<IActionResult> GetBidsAsync()
    {
        return Ok();
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetBidByIdAsync([FromRoute] string id)
    {
        return Ok();
    }
}