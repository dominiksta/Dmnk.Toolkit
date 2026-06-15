using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Dmnk.LocalizedDataAnnotations.MessageProvider;

namespace Dmnk.LocalizedDataAnnotations;

/// <summary>
/// A localized version of <see cref="IValidator"/> based on the messages provided
/// by <paramref name="messageProvider"/>.
/// </summary>
public class LocalizedValidator(IDefaultValidationMessageProvider messageProvider) : IValidator
{
    public bool TryValidateObject(
        object instance, 
        ValidationContext validationContext,
        ICollection<ValidationResult> validationResults, 
        bool validateAllProperties = false)
    {
        throw new System.NotImplementedException();
    }

    public bool TryValidateProperty(
        object? value, 
        ValidationContext validationContext,
        ICollection<ValidationResult> validationResults)
    {
        throw new System.NotImplementedException();
    }

    public bool TryValidateValue(
        object? value, 
        ValidationContext validationContext,
        ICollection<ValidationResult> validationResults, 
        IEnumerable<ValidationAttribute> validationAttributes)
    {
        throw new System.NotImplementedException();
    }
}