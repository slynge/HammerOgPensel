using System.ComponentModel.DataAnnotations;

namespace AuctionAPI.Context.Models;

public class OutboxMessageDb
{
    [Key]
    public Guid Id { get; set; }
    public EventType EventType { get; set; }
    public bool IsProcessed { get; set; }
    public string Payload { get; set; }
    
    public OutboxMessageDb()
    {
        Id = Guid.NewGuid();
        IsProcessed = false;
    }
}