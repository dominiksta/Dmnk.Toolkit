using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Dmnk.LocalizedDataAnnotations.MessageProvider;

namespace Dmnk.LocalizedDataAnnotations;

/// <summary>
/// A localized version of <see cref="IValidator"/> based on the messages provided
/// by the passed <see cref="IDefaultValidationMessageProvider"/>.
/// <p>
/// If no message provider is passed, the default best-effort provider will be used, for which
/// see the package documentation.
/// </p>
/// </summary>
public class LocalizedValidator : IValidator
{
    private readonly MessageLocalizer _localizer;
    
    /// <summary> Instantiate a new <see cref="LocalizedValidator"/>. </summary>
    public LocalizedValidator(IDefaultValidationMessageProvider? messageProvider = null)
    {
        var messageProvider1 = 
            messageProvider ?? new DefaultBestEffortValidationMessageProvider();
        _localizer = new MessageLocalizer(messageProvider1);
    }
    
    // ======================================================================
    // API
    // ======================================================================
    
    /// <summary> <inheritdoc/> </summary>
    public bool TryValidateObject(
        object instance,
        ValidationContext validationContext,
        ICollection<ValidationResult> validationResults,
        bool validateAllProperties = false)
    {
        bool isValid = true;

        // Type-level validation attributes
        var typeAttrs = instance.GetType()
            .GetCustomAttributes(inherit: true)
            .OfType<ValidationAttribute>();
        
        if (!TryValidateValue(instance, validationContext, validationResults, typeAttrs))
            isValid = false;

        // Property-level validation
        foreach (var property in 
                 instance.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            if (!property.CanRead) continue;

            var propAttrs = property.GetCustomAttributes(inherit: true)
                .OfType<ValidationAttribute>()
                .ToList();
            if (propAttrs.Count == 0) continue;

            List<ValidationAttribute> attrsToValidate = validateAllProperties
                ? propAttrs
                : propAttrs.Where(a => a is RequiredAttribute).ToList();

            if (!attrsToValidate.Any()) continue;

            object? propValue;
            try { propValue = property.GetValue(instance); }
            catch { continue; }

            var propContext = new ValidationContext(instance, validationContext, validationContext.Items)
            {
                MemberName = property.Name
            };

            if (!TryValidateValue(propValue, propContext, validationResults, attrsToValidate))
                isValid = false;
        }

        // IValidatableObject support — matches Validator behavior: only called when
        // all attribute validations pass.
        if (isValid && instance is IValidatableObject validatable)
        {
            foreach (var vr in validatable.Validate(validationContext))
            {
                if (vr == ValidationResult.Success) continue;
                isValid = false;
                validationResults.Add(vr);
            }
        }

        return isValid;
    }

    /// <summary> <inheritdoc/> </summary>
    public bool TryValidateProperty(
        object? value,
        ValidationContext validationContext,
        ICollection<ValidationResult> validationResults)
    {
        if (validationContext.MemberName == null) throw new ArgumentException(
            "ValidationContext.MemberName must be set.", nameof(validationContext));

        var attrs = validationContext.ObjectType
            .GetProperty(validationContext.MemberName, BindingFlags.Public | BindingFlags.Instance)
            ?.GetCustomAttributes(inherit: true)
            .OfType<ValidationAttribute>()
            ?? Enumerable.Empty<ValidationAttribute>();

        return TryValidateValue(value, validationContext, validationResults, attrs);
    }

    /// <summary> <inheritdoc/> </summary>
    public bool TryValidateValue(
        object? value,
        ValidationContext validationContext,
        ICollection<ValidationResult> validationResults,
        IEnumerable<ValidationAttribute> validationAttributes)
    {
        bool isValid = true;

        foreach (var attr in validationAttributes)
        {
            var result = attr.GetValidationResult(value, validationContext);
            if (result == null || result == ValidationResult.Success) continue;

            isValid = false;
            validationResults.Add(LocalizeResult(attr, validationContext, result));
        }

        return isValid;
    }
    
    // ======================================================================
    // Helpers
    // ======================================================================
    
    private readonly Lazy<FieldInfo> _defaultErrorMessageProperty = new(
        () => typeof(ValidationAttribute).GetField(
            "_defaultErrorMessage", BindingFlags.NonPublic | BindingFlags.Instance) 
              ?? throw new InvalidOperationException(
                  "Could not find _defaultErrorMessage property via reflection."));

    private bool HasUserDefinedErrorMessage(ValidationAttribute attr)
    {
        var defaultErrorMessage = (string?)_defaultErrorMessageProperty.Value.GetValue(attr);
        return attr.ErrorMessage != defaultErrorMessage;
    }
    
    private ValidationResult LocalizeResult(
        ValidationAttribute attr,
        ValidationContext context,
        ValidationResult original)
    {
        if (HasUserDefinedErrorMessage(attr)) return original;

        string fieldName = context.DisplayName ?? context.MemberName ?? string.Empty;
        string? localizedMessage = _localizer.GetLocalizedMessage(attr, fieldName) 
                                   ?? original.ErrorMessage;

        return new ValidationResult(localizedMessage, original.MemberNames);
    }

}