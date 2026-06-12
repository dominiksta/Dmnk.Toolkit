using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Dmnk.LocalizedDataAnnotations;

public interface IValidator
{
    /// <summary>
    /// See
    /// <see cref="Validator.TryValidateObject(object, ValidationContext, ICollection&lt;ValidationResult&gt;)"/>
    /// </summary>
    bool TryValidateObject(
        object instance,
        ValidationContext validationContext,
        ICollection<ValidationResult> validationResults,
        bool validateAllProperties = false);

    /// <summary>
    /// See
    /// <see cref="Validator.TryValidateProperty(object, ValidationContext, ICollection&lt;ValidationResult&gt;)"/>
    /// </summary>
    bool TryValidateProperty(
        object? value,
        ValidationContext validationContext,
        ICollection<ValidationResult> validationResults);

    /// <summary>
    /// See
    /// <see cref="Validator.TryValidateValue(object, ValidationContext, ICollection&lt;ValidationResult&gt;, IEnumerable&lt;ValidationAttribute&gt;)"/>
    /// </summary>
    bool TryValidateValue(
        object? value,
        ValidationContext validationContext,
        ICollection<ValidationResult> validationResults,
        IEnumerable<ValidationAttribute> validationAttributes);
}
