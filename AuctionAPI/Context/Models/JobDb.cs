using System.ComponentModel.DataAnnotations;
using DTO.Job;

namespace AuctionAPI.Context.Models;

public class JobDb
{
    [Key]
    public Guid Id { get; set; }
    public string Description { get; set; }
    public string Email { get; set; }
    public string Zip { get; set; }
    public Category Category { get; set; }
    public bool Open { get; set; }
    public IEnumerable<BidDb> Bids { get; set; }

    public JobDb()
    {
        Open = true;
        Bids = [];
    }
}