using System.ComponentModel.DataAnnotations;

namespace AuctionAPI.Context.Models;

public class OutboxMessageDb
{
    [Key]
    public Guid Id { get; set; }
    public string EventType { get; set; }
    public bool IsProcessed { get; set; }
    public string Payload { get; set; }
    
    public OutboxMessageDb()
    {
        IsProcessed = false;
    }
}