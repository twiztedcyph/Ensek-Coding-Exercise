var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.EnsekCodingExercise_ApiService>("apiservice");

builder.AddProject<Projects.EnsekCodingExercise_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService);

builder.Build().Run();
