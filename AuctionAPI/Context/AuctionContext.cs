using AuctionAPI.Context.Models;
using Microsoft.EntityFrameworkCore;

namespace AuctionAPI.Context;

internal class AuctionContext : DbContext
{
    public AuctionContext(DbContextOptions options) : base(options)
    {
    }
    
    public DbSet<JobDb> Jobs { get; set; }
    public DbSet<BidDb> Bids { get; set; }
    public DbSet<OutboxMessageDb> OutboxMessages { get; set; }
}