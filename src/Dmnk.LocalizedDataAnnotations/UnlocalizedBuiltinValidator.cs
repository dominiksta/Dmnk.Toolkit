using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Dmnk.LocalizedDataAnnotations;

/// <summary>
/// Implements <see cref="IValidator"/> by simply delegating to the "real" <see cref="Validator"/>.
/// </summary>
public sealed class UnlocalizedBuiltinValidator : IValidator
{
    /// <summary> <inheritdoc/> </summary>
    public bool TryValidateObject(
        object instance, 
        ValidationContext validationContext, 
        ICollection<ValidationResult> validationResults,
        bool validateAllProperties = false) =>
        Validator.TryValidateObject(
            instance, validationContext, validationResults, validateAllProperties);

    /// <summary> <inheritdoc/> </summary>
    public bool TryValidateProperty(
        object? value,
        ValidationContext validationContext,
        ICollection<ValidationResult> validationResults)
        => Validator.TryValidateProperty(value, validationContext, validationResults);

    /// <summary> <inheritdoc/> </summary>
    public bool TryValidateValue(
        object? value, 
        ValidationContext validationContext, 
        ICollection<ValidationResult> validationResults,
        IEnumerable<ValidationAttribute> validationAttributes) 
        => Validator.TryValidateValue(
            value, validationContext, validationResults, validationAttributes);
}
