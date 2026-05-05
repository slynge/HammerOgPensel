namespace AuctionAPI.Context.Models;

public class BidDb
{
    public Guid Id { get; set; }
    public decimal Price { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public JobDb Job { get; set; }
    public Guid JobId { get; set; }

    public BidDb()
    {
    }
}