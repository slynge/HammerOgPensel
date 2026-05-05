var builder = DistributedApplication.CreateBuilder(args);

var username = builder.AddParameter("username", "guest");
var password = builder.AddParameter("password", "guest");
var rabbitmq = builder.AddRabbitMQ("messaging", username, password)
    .WithLifetime(ContainerLifetime.Persistent)
    .WithManagementPlugin();

var pgpassword = builder.AddParameter("postgresql-password", "1234asdf");
var postgres = builder.AddPostgres("postgres", password: pgpassword)
    .WithPgWeb()
    .WithDataVolume(isReadOnly: false)
    .WithLifetime(ContainerLifetime.Persistent);
var ordersDb = postgres.AddDatabase("AuctionDb");

builder.AddProject<Projects.AuctionAPI>("Auction-AuctionAPI")
    .WithReference(rabbitmq)
    .WaitFor(rabbitmq)
    .WithReference(ordersDb)
    .WaitFor(ordersDb);

builder.AddProject<Projects.NotificationService>("Notification-Service")
    .WithReference(rabbitmq)
    .WaitFor(rabbitmq);

builder.Build().Run();
