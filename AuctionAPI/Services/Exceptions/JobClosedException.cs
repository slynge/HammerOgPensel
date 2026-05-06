namespace AuctionAPI.Services.Exceptions;

public class JobClosedException : Exception
{
    public string JobId { get; }
    
    public JobClosedException(string jobId) 
        : base($"Job with id '{jobId}' is closed and cannot accept new bids.")
    {
        JobId = jobId;
    }
}