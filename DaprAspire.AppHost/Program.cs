using Aspire.Hosting;

using CommunityToolkit.Aspire.Hosting.Dapr;

IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

//var redis = builder.AddRedis("redis");

IResourceBuilder<MongoDBServerResource> mongodb = builder.AddMongoDB("mongodb").WithLifetime(ContainerLifetime.Persistent);

builder.AddDapr();
builder.AddDaprPubSub("pubsub");

builder.AddProject<Projects.DaprAspire_Entries_Api>("dapraspire-entries-api")
       .WithDaprSidecar()
       .WithReference(mongodb);

builder.AddProject<Projects.DaprAspire_ConsolidationApi>("dapraspire-consolidationapi")
       .WithDaprSidecar()
       .WithReference(mongodb);

DaprSidecarOptions options = new DaprSidecarOptions
{
    DaprHttpPort = 3500,
};

IResourceBuilder<ProjectResource> gateway = builder.AddProject<Projects.DaprAspire_Gateway>("dapraspire-gateway")
                                                   //.WithEndpoint(name: "gateway", port: 5055, targetPort: 8080, scheme: "http")
                                                   .WithDaprSidecar(options);

builder.Build().Run();
