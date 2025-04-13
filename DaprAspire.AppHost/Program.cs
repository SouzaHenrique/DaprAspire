var builder = DistributedApplication.CreateBuilder(args);

var redis = builder.AddRedis("redis");

var mongodb = builder.AddMongoDB("mongodb");

builder.AddDapr();

builder.AddProject<Projects.DaprAspire_Entries_Api>("dapraspire-entries-api")
       .WithDaprSidecar()
       .WithReference(mongodb);

builder.AddProject<Projects.DaprAspire_ConsolidationApi>("dapraspire-consolidationapi")
       .WithDaprSidecar();

builder.Build().Run();
