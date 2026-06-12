using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Dmnk.LocalizedDataAnnotations;

public interface IValidationMessageProvider
{
    string GetMessage(
        ValidationAttribute attribute,
        ValidationContext validationContext,
        object? value,
        CultureInfo culture);
}
