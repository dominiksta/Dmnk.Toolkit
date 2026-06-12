using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Dmnk.LocalizedDataAnnotations;

public sealed class UnlocalizedBuiltinValidator : IValidator
{
    public bool TryValidateObject(
        object instance, 
        ValidationContext validationContext, 
        ICollection<ValidationResult> validationResults,
        bool validateAllProperties = false) =>
        Validator.TryValidateObject(instance, validationContext, validationResults, validateAllProperties);

    public bool TryValidateProperty(
        object? value,
        ValidationContext validationContext,
        ICollection<ValidationResult> validationResults)
        => Validator.TryValidateProperty(value, validationContext, validationResults);

    public bool TryValidateValue(
        object? value, 
        ValidationContext validationContext, 
        ICollection<ValidationResult> validationResults,
        IEnumerable<ValidationAttribute> validationAttributes) 
        => Validator.TryValidateValue(value, validationContext, validationResults, validationAttributes);
}
