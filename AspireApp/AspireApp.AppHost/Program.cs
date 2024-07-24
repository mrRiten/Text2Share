var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.TextMicroService_API>("TextApiService");
builder.AddProject<Projects.UserMicroService_API>("UserApiService");
builder.AddProject<Projects.AuthorizeMicroService_API>("AuthorizeApiService");
builder.AddProject<Projects.EmailMicroService_API>("EmailApiService");

builder.Build().Run();
