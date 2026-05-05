using DTO.Bid;

namespace DTO.Job;

public record JobDto(string? Id, string Description, string Zip, Category Category, bool? Open, IEnumerable<BidDto>? Bids);