namespace FE.IntegrationTests.Repro.API.Core;

public sealed class Error
{
    public string Code { get; }
    public string Message { get; }

    public Error(string code, string message)
    {
        ArgumentException.ThrowIfNullOrEmpty(code?.Trim());
        ArgumentException.ThrowIfNullOrEmpty(message?.Trim());

        Code = code;
        Message = message;
    }
}

public static class Errors
{
    public static class General
    {
        public static Error NotFound(long? id = null)
        {
            string forId = id == null ? string.Empty : $" for Id '{id}'";
            return new Error(ErrorCodes.NotFound, $"Record not found{forId}");
        }

        public static Error InternalServerError(string message) =>
            new(ErrorCodes.InternalServerError, message);

        public static Error Unauthorized() =>
            new(ErrorCodes.Unauthorized, "User is not authorized.");

        public static Error NotAllowed() =>
            new(ErrorCodes.NotAllowed, "User doesn't have necessary permissions.");

        public static Error ValueIsInvalid() =>
            new(ErrorCodes.ValueIsInvalid, "Value is invalid");

        public static Error ValueIsRequired() =>
            new(ErrorCodes.ValueIsRequired, "Value is required");

        public static Error ValueIsTooSmall(int min, bool inclusive = true)
        {
            var label = inclusive ? $"greater than or equal to {min}." : $"greater than {min}.";
            return new Error(ErrorCodes.ValueIsTooSmall, $"Value must be {label}");
        }

        public static Error InvalidLength(string? name = null)
        {
            string label = string.IsNullOrWhiteSpace(name) ? " " : $" {name} ";
            return new Error(ErrorCodes.InvalidLength, $"Invalid{label}length");
        }
    }
}

public static class ErrorCodes
{
    public const string NotFound = "error.not.found";
    public const string InternalServerError = "error.internal.server";
    public const string Unauthorized = "error.unauthorized";
    public const string NotAllowed = "error.not.allowed";
    public const string ValueIsInvalid = "error.value.invalid";
    public const string ValueIsRequired = "error.value.required";
    public const string ValueIsTooSmall = "error.value.too.small";
    public const string InvalidLength = "error.invalid.length";
}
