using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace FE.IntegrationTests.Repro.API.Bootstrap;

/// <summary>
/// Extensions for global exception handling.
/// </summary>
internal static class ErrorHandlingBootstrap
{
    /// <summary>
    /// registers the default global exception handler which will log the exceptions on the server and return a user-friendly json response to the client when unhandled exceptions occur.
    /// TIP: when using this exception handler, you may want to turn off the asp.net core exception middleware logging to avoid duplication like so:
    /// <code>
    /// "Logging": { "LogLevel": { "Default": "Warning", "Microsoft.AspNetCore.Diagnostics.ExceptionHandlerMiddleware": "None" } }
    /// </code>
    /// </summary>
    /// <param name="app">Application builder.</param>
    /// <param name="logStructuredException">Set to true if you'd like to log the error in a structured manner.</param>
    /// <returns>Application builder.</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2254:Template should be a static expression", Justification = "That log method will be used only for local development")]
    public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder app, bool logStructuredException = false)
    {
        app.UseExceptionHandler(errApp =>
        {
            errApp.Run(async ctx =>
            {
                var exHandlerFeature = ctx.Features.Get<IExceptionHandlerFeature>();
                if (exHandlerFeature is not null)
                {
                    var logger = ctx.Resolve<ILogger<ExceptionHandler>>();
                    var http = exHandlerFeature.Endpoint?.DisplayName?.Split(" => ")[0];
                    var type = exHandlerFeature.Error.GetType().Name;
                    var error = exHandlerFeature.Error.StackTrace;

                    if (logStructuredException)
                    {
                        logger.LogError("{@http}{@type}{@reason}{@exception}", http, type, error, exHandlerFeature.Error)
                        ;
                    }
                    else
                    {
                        var logMsg =
$@"================================= 
{http} 
TYPE: {type} 
REASON: {error} 
--------------------------------- 
{exHandlerFeature.Error.StackTrace}";
                        logger.LogError(logMsg);
                    }

                    ctx.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    ctx.Response.ContentType = "application/problem+json";

                    var env = app.ApplicationServices.GetRequiredService<IWebHostEnvironment>();
                    var msg = env.IsDevelopment() ? $"Error: {error}" : "Internal server error";

                    var err = Errors.General.InternalServerError(msg);
                    await ctx.Response.WriteAsJsonAsync(Envelope.Error(err, null));
                }
            });
        });

        return app;
    }

    /// <summary>
    /// Registers handlers for Unauthorized and Forbidden status codes to return a standardized API response.
    /// </summary>
    /// <param name="app">Application builder</param>
    /// <returns>Application builder</returns>
    public static IApplicationBuilder UseCustomAuthorizationErrorHandlers(this IApplicationBuilder app)
    {
        app.UseStatusCodePages(async context =>
        {
            switch (context.HttpContext.Response.StatusCode)
            {
                case (int)HttpStatusCode.Unauthorized:
                    {
                        var envelope = Envelope.Error(Errors.General.Unauthorized(), null);
                        await context.HttpContext.Response.WriteAsJsonAsync(envelope);
                    }
                    break;
                case (int)HttpStatusCode.Forbidden:
                    {
                        var envelope = Envelope.Error(Errors.General.NotAllowed(), null);
                        await context.HttpContext.Response.WriteAsJsonAsync(envelope);
                    }
                    break;
            }
        });

        return app;
    }
}
internal class ExceptionHandler { }
