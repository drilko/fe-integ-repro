using FastEndpoints.Swagger;

namespace FE.IntegrationTests.Repro.API.Bootstrap;

public static class MinimalApiBootstrap
{
    public static WebApplicationBuilder AddMinimalApis(this WebApplicationBuilder builder)
    {
        builder.Services.AddFastEndpoints();

        AddSwaggerDocuments(builder.Services);

        return builder;
    }

    public static WebApplication UseMinimalApis(this WebApplication app)
    {
        app.UseFastEndpoints(c =>
        {
            c.Endpoints.RoutePrefix = "api";

            c.Versioning.Prefix = "v";
            c.Versioning.PrependToRoute = true;

            c.Errors.ResponseBuilder = SendValidationError;
            c.Errors.ProducesMetadataType = typeof(Envelope);
        });

        if (app.Environment.IsDevelopment())
        {
            app.UseSwaggerGen();
        }

        return app;
    }

    private static void AddSwaggerDocuments(IServiceCollection services)
    {
        services.SwaggerDocument(o =>
        {
            o.MaxEndpointVersion = 1;
            o.DocumentSettings = s =>
            {
                s.Title = "Repro API";
                s.DocumentName = "Release 1.0";
                s.Version = "v1";
            };

            o.EnableJWTBearerAuth = false;
        });
    }

    private static Envelope SendValidationError(List<ValidationFailure> validationFailures, HttpContext context, int statusCode)
    {
        var validationFailure = validationFailures[0];

        var err = new Error(validationFailure.ErrorCode, validationFailure.ErrorMessage);
        var envelope = Envelope.Error(err, validationFailure.PropertyName);

        return envelope;
    }
}
