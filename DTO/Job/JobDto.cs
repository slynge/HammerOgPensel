using DTO.Bid;

namespace DTO.Job;

public record JobDto(string? Id, string Description, string Email, string Zip, Category Category, bool? Open, IEnumerable<BidDto>? Bids);