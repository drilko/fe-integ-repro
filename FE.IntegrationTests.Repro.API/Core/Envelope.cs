using System.Diagnostics;

namespace FE.IntegrationTests.Repro.API.Core;
public class Envelope : Envelope<object>
{
    public Envelope() : base()
    { }

    public Envelope(object? result, Error? error, string? invalidField)
        : base(result, error, invalidField)
    { }

    public static new Envelope Ok(object result) => new(result, null, null);
    public static new Envelope Error(Error error, string? invalidField) => new(null, error, invalidField);
}

public class Envelope<T>
    where T : class
{
    public T? Result { get; init; }
    public string? ErrorCode { get; init; }
    public string? ErrorMessage { get; init; }
    public string? InvalidField { get; init; }
    public DateTime TimeGenerated { get; init; }
    public string TraceId { get; init; }

    public Envelope()
    {
        TimeGenerated = DateTime.UtcNow;
        TraceId = GetTraceId();
    }

    protected Envelope(T? result, Error? error, string? invalidField) : this()
    {
        Result = result;
        ErrorCode = error?.Code;
        ErrorMessage = error?.Message;
        InvalidField = invalidField;
    }

    public static Envelope<T> Ok(T? result = null)
    {
        return new Envelope<T>(result, null, null);
    }

    public static Envelope<T> Error(Error error, string? invalidField)
    {
        return new Envelope<T>(null, error, invalidField);
    }

    public static implicit operator Envelope<T>(T t) => Ok(t);

    private static string GetTraceId()
    {
        var activity = Activity.Current;

        if (activity is null)
        {
            return string.Empty;
        }

        return activity.IdFormat switch
        {
            ActivityIdFormat.Hierarchical => activity.RootId ?? string.Empty,
            ActivityIdFormat.W3C => activity.TraceId.ToHexString(),
            ActivityIdFormat.Unknown => string.Empty,
            _ => string.Empty,
        };
    }
}
