using Aspire.Hosting;

using CommunityToolkit.Aspire.Hosting.Dapr;

IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

builder.AddDapr();
builder.AddDaprPubSub("pubsub");

IResourceBuilder<MongoDBServerResource> entriesDb = builder.AddMongoDB("entries-db")
                                                           .WithLifetime(ContainerLifetime.Persistent);

IResourceBuilder<MongoDBServerResource> consolidationDb = builder.AddMongoDB("consolidation-db")
                                                                 .WithLifetime(ContainerLifetime.Persistent);

IResourceBuilder<MongoDBServerResource> identityDb = builder.AddMongoDB("identity-db")
                                                                 .WithLifetime(ContainerLifetime.Persistent);

builder.AddProject<Projects.DaprAspire_Entries_Api>("dapraspire-entries-api")
       .WithDaprSidecar()
       .WithReference(entriesDb);

builder.AddProject<Projects.DaprAspire_ConsolidationApi>("dapraspire-consolidationapi")
       .WithDaprSidecar()
       .WithReference(consolidationDb);


builder.AddProject<Projects.DaprAspire_IdentityService_Api>("dapraspire-identityservice-api")
       .WithDaprSidecar()
       .WithReference(identityDb);


builder.AddProject<Projects.DaprAspire_Gateway>("dapraspire-gateway")
       .WithDaprSidecar(new DaprSidecarOptions
       {
           DaprHttpPort = 3500,
       });

builder.AddProject<Projects.DaprAspire_FrontEnd>("dapraspire-frontend");


builder.Build().Run();
