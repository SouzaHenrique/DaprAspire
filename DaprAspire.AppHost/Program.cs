using Aspire.Hosting;

using CommunityToolkit.Aspire.Hosting.Dapr;

IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

builder.AddDapr();
builder.AddDaprPubSub("pubsub");

IResourceBuilder<MongoDBServerResource> mongodb = builder.AddMongoDB("mongodb")
                                                         .WithLifetime(ContainerLifetime.Persistent);

builder.AddProject<Projects.DaprAspire_Entries_Api>("dapraspire-entries-api")
       .WithDaprSidecar()
       .WithReference(mongodb);

builder.AddProject<Projects.DaprAspire_ConsolidationApi>("dapraspire-consolidationapi")
       .WithDaprSidecar()
       .WithReference(mongodb);

builder.AddProject<Projects.DaprAspire_IdentityService_Api>("dapraspire-identityservice-api")
       .WithDaprSidecar()
       .WithReference(mongodb);

var gatewaySideCarOptions = new DaprSidecarOptions
{
    DaprHttpPort = 3500,
};

IResourceBuilder<ProjectResource> gateway = builder.AddProject<Projects.DaprAspire_Gateway>("dapraspire-gateway")
                                                   .WithDaprSidecar(gatewaySideCarOptions);

builder.Build().Run();
