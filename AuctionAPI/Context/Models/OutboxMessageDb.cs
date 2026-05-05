using System.ComponentModel.DataAnnotations;

namespace AuctionAPI.Context.Models;

internal class OutboxMessageDb
{
    [Key]
    public int Id { get; set; }

    public OutboxMessageDb()
    {
    }
}