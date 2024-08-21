using FluentValidation;

namespace Core.Extensions;

public static class FluentValidationExtensions
{
    public static IRuleBuilderOptions<T, TProperty> WithDisplayName<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder, string displayName)
    {
        var ruleBuilderOptions = (IRuleBuilderOptions<T, TProperty>)ruleBuilder;

        return ruleBuilderOptions
            .OverridePropertyName(string.Empty)
            .WithName(displayName);
    }
}