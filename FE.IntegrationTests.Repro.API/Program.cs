using FE.IntegrationTests.Repro.API.Bootstrap;
using FE.IntegrationTests.Repro.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.AddMinimalApis();

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

builder.Services.AddInfrastructure(builder.Configuration)
                .AddMapping();


var app = builder.Build();

app.UseCustomExceptionHandler(app.Configuration.GetValue<bool>("GlobalErrorHandler:UseStructuredLogging"))
   .UseCustomAuthorizationErrorHandlers();

app.UseAuthentication();
app.UseAuthorization();

app.UseMinimalApis();

app.Run();
