using FluentValidation;

namespace FE.IntegrationTests.Repro.API.Extensions;

public static class ValidationExtensions
{
    public static IRuleBuilderOptions<T, TProperty> WithError<T, TProperty>(this IRuleBuilderOptions<T, TProperty> rule, Error error)
    {
        ArgumentNullException.ThrowIfNull(nameof(error));

        rule.WithErrorCode(error.Code);
        rule.WithMessage(error.Message);

        return rule;
    }
}
