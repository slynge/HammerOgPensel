using System.Text;
using System.Text.Json;
using DTO.Bid;
using DTO.Job;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace NotificationService;

public class NotificationWorker : BackgroundService
{
    private readonly IConnection _connection;
    private readonly ILogger<NotificationWorker> _logger;
    private readonly List<string> _guids;
    public NotificationWorker(IConnection connection, ILogger<NotificationWorker> logger)
    {
        _connection = connection;
        _logger = logger;
        _guids = [];
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await using IChannel channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);
        await channel.QueueDeclareAsync(
            queue: "auction.closed",
            durable: true,
            exclusive: false,
            cancellationToken: stoppingToken);
        
            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += (o, ea) =>
            {
                try
                {
                    var body = Encoding.UTF8.GetString(ea.Body.ToArray());
                    var jobDto = JsonSerializer.Deserialize<JobDto>(body);
                
                    if (!_guids.Contains(jobDto!.Id!))
                    {
                        _guids.Add(jobDto.Id!);
                        SendEmails(jobDto);
                    }

                    return Task.CompletedTask;
                }
                catch (Exception exception)
                {
                    return Task.FromException(exception);
                }
            };
            
            await channel.BasicConsumeAsync(
                queue: "auction.closed",
                autoAck: true,
                consumer: consumer, 
                cancellationToken: stoppingToken);
            
            await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    private void SendEmails(JobDto jobDto)
    {
        var sortedBidDtos = jobDto.Bids!.OrderBy(b => b.Price).ToList();
        _logger.LogInformation(SendEmailToCustomer(jobDto, sortedBidDtos.First()));
        _logger.LogInformation(SendEmailToWinner(sortedBidDtos.First()));
        _logger.LogInformation(SendEmailToLosers(sortedBidDtos.GetRange(1, 2)));
    }
    

    // Following methods are AI-generated.
    private string SendEmailToCustomer(JobDto jobDto, BidDto winningBidDto)
    {
        return $@"Hej

            Tak for at bruge vores service. Dit job er nu afsluttet, og vinderen af buddet er fundet.

            Jobdetaljer:
            - Beskrivelse: {jobDto.Description}
            - Kategori: {jobDto.Category}
            - Postnummer: {jobDto.Zip}

            Vinderbud:
            - Navn: {winningBidDto.Name}
            - Email: {winningBidDto.Email}
            - Pris: {winningBidDto.Price:C}

            Du kan kontakte vinderen direkte på ovenstående email for at aftale det videre forløb.

            Med venlig hilsen
            JobService";
    }
    
    private string SendEmailToWinner(BidDto winningBidDto)
    {
        return $@"Hej {winningBidDto.Name}

            Tillykke! Dit bud er blevet accepteret, og du har vundet jobbet.

            Dit vindende bud:
            - Pris: {winningBidDto.Price:C}

            Kunden vil kontakte dig på denne email for at aftale det videre forløb. Du kan også selv tage kontakt, hvis du ønsker det.

            Med venlig hilsen
            JobService";
    }

    private string SendEmailToLosers(List<BidDto> losingBidsDtos)
    {
        var recipients = string.Join(", ", losingBidsDtos.Select(b => b.Email));

        return $@"Hej

            Tak for dit bud på jobbet. Vi må desværre meddele, at en anden bydende er blevet valgt denne gang.

            Vi håber, du vil byde igen på fremtidige opgaver.

            Med venlig hilsen
            JobService

            (Sendt til: {recipients})";
    }
}