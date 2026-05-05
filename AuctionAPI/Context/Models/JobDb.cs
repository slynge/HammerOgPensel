using System.ComponentModel.DataAnnotations;
using DTO.Job;

namespace AuctionAPI.Context.Models;

internal class JobDb
{
    [Key]
    public Guid Id { get; set; }
    public string Description { get; set; }
    public string Zip { get; set; }
    public Category Category { get; set; }
    public Status Status { get; set; }
    public IEnumerable<BidDb> Bids { get; set; }

    public JobDb()
    {
        Status = Status.OPEN;
        Bids = [];
    }
}