using System.Net;

namespace FE.IntegrationTests.Repro.API.Core;

public abstract class ApiEndpoint<TRequest, TResponse> : Endpoint<TRequest, Envelope<TResponse>>
    where TRequest : notnull
    where TResponse : class
{
    private MyDbContext? _context;
    public MyDbContext Context
    {
        get
        {
            _context ??= HttpContext!.RequestServices.GetRequiredService<MyDbContext>();
            return _context;
        }
    }

    protected async Task SendNotFoundAsync(int? id = null, CancellationToken cancellation = default)
    {
        var envelope = Envelope<TResponse>.Error(Errors.General.NotFound(id), null);
        await SendAsync(envelope, (int)HttpStatusCode.NotFound, cancellation);
    }

    protected new Task SendNotFoundAsync(CancellationToken cancellation = default)
    {
        return SendNotFoundAsync(null, cancellation);
    }

    protected new async Task SendUnauthorizedAsync(CancellationToken cancellation = default)
    {
        var envelope = Envelope<TResponse>.Error(Errors.General.Unauthorized(), null);
        await SendAsync(envelope, (int)HttpStatusCode.Unauthorized, cancellation);
    }

    protected new async Task SendForbiddenAsync(CancellationToken cancellation = default)
    {
        var envelope = Envelope<TResponse>.Error(Errors.General.NotAllowed(), null);
        await SendAsync(envelope, (int)HttpStatusCode.Forbidden, cancellation);
    }

    protected new async Task SendOkAsync(CancellationToken cancellation = default)
    {
        var envelope = Envelope<TResponse>.Ok();
        await SendAsync(envelope, (int)HttpStatusCode.OK, cancellation);
    }

    protected void ThrowIfError(string propertyName, Error? error)
    {
        if (error != null)
        {
            ThrowError(propertyName, error);
        }
    }

    protected void ThrowError(string propertyName, Error error)
    {
        ThrowError(new ValidationFailure(propertyName, error.Message)
        {
            ErrorCode = error.Code,
        });
    }
}
