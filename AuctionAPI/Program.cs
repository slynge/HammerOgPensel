using AuctionAPI.Context;
using AuctionAPI.Context.Repositories.Bid;
using AuctionAPI.Context.Repositories.Job;
using AuctionAPI.Context.Repositories.Outbox;
using AuctionAPI.Services.Bid;
using AuctionAPI.Services.Job;
using AuctionAPI.Services.Outbox;

namespace AuctionAPI;

internal class Program
{
    public async static Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.AddServiceDefaults();
        builder.AddRabbitMQClient("messaging");
        builder.AddNpgsqlDbContext<AuctionContext>("AuctionDb");
        builder.Services.AddScoped<IJobService, JobService>();
        builder.Services.AddScoped<IBidService, BidService>();
        builder.Services.AddHostedService<OutboxService>();
        
        builder.Services.AddScoped<IJobRepository, JobRepository>();
        builder.Services.AddScoped<IBidRepository, BidRepository>();
        builder.Services.AddScoped<IOutboxRepository, OutboxRepository>();
        
        builder.Services.AddAuthorization();
        builder.Services.AddControllers(options =>
        {
            options.SuppressAsyncSuffixInActionNames = false;
        });
        

        var app = builder.Build();

        app.MapDefaultEndpoints();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            // Ensure database is created and seeded
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AuctionContext>();
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();
        }

        app.UseHttpsRedirection();

        app.MapControllers();

        app.UseAuthorization();
        
        await app.RunAsync();
    }
}