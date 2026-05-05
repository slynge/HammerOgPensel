using DTO.Bid;

namespace DTO.Job;

public record JobDto(string? Id, string Description, string Zip, Category Category, Status? Status, IEnumerable<BidDto>? Bids);