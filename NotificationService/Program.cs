namespace NotificationService;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder();
        builder.AddServiceDefaults();
        builder.AddRabbitMQClient("messaging");
        builder.Services.AddHostedService<NotificationWorker>();

        var host = builder.Build();
        await host.RunAsync();
    }
}