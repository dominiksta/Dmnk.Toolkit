using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using Dmnk.LocalizedDataAnnotations.MessageProvider;

namespace Dmnk.LocalizedDataAnnotations;

internal class MessageLocalizer(IDefaultValidationMessageProvider messageProvider)
{
    public string? GetLocalizedMessage(ValidationAttribute attr, string fieldName)
    {
        return attr switch
        {
#if NET8_0_OR_GREATER
            AllowedValuesAttribute => Format1(messageProvider.AllowedValues, fieldName),
            DeniedValuesAttribute => Format1(messageProvider.DeniedValues, fieldName),
            Base64StringAttribute => Format1(messageProvider.Base64String, fieldName),
#endif
#if NET8_0_OR_GREATER
            LengthAttribute la => FormatLength(la, fieldName),
#endif
            FileExtensionsAttribute fea => FormatFileExtensions(fea, fieldName),
            CreditCardAttribute => Format1(messageProvider.DataTypeCreditCard, fieldName),
            EmailAddressAttribute => Format1(messageProvider.DataTypeEmailAddress, fieldName),
            PhoneAttribute => Format1(messageProvider.DataTypePhone, fieldName),
            UrlAttribute => Format1(messageProvider.DataTypeUrl, fieldName),
            
            CompareAttribute ca => FormatCompare(ca, fieldName),
            RangeAttribute ra => FormatRange(ra, fieldName),
            RegularExpressionAttribute rea => Format2(messageProvider.RegularExpression, fieldName, rea.Pattern),
            StringLengthAttribute sla => FormatStringLength(sla, fieldName),
            MaxLengthAttribute mla => Format2(messageProvider.MaxLength, fieldName, mla.Length),
            MinLengthAttribute mla => Format2(messageProvider.MinLength, fieldName, mla.Length),
            RequiredAttribute => Format1(messageProvider.Required, fieldName),
            // EnumDataTypeAttribute has no dedicated message; the generic fallback is appropriate.
            EnumDataTypeAttribute => Format1(messageProvider.Fallback, fieldName),
            // For CustomValidationAttribute and other unknown attributes, return null so the caller
            // uses the attribute's own message (which may come from a custom validator method).
            _ => null,
        };
    }

    private static string? Format1(string? template, string fieldName) =>
        template != null ? string.Format(CultureInfo.CurrentCulture, template, fieldName) : null;

    private static string? Format2(string? template, string fieldName, object? p1) =>
        template != null ? string.Format(CultureInfo.CurrentCulture, template, fieldName, p1) : null;

    private static string? Format3(string? template, string fieldName, object? p1, object? p2) =>
        template != null ? string.Format(CultureInfo.CurrentCulture, template, fieldName, p1, p2) : null;

    private string? FormatCompare(CompareAttribute ca, string fieldName) =>
        Format2(messageProvider.Compare, fieldName, ca.OtherProperty);
    
    private string? FormatFileExtensions(FileExtensionsAttribute fea, string fieldName)
    {
        string[] extensions = fea.Extensions
            .Split(',').Select(e => e.Trim())
            .OrderBy(e => e, StringComparer.CurrentCultureIgnoreCase)
            .Select(e => e.StartsWith(".") ? e : "." + e)
            .ToArray();
        string extensionsFormatted = string.Join(", ", extensions);
        return Format2(messageProvider.DataTypeFileExtension, fieldName, extensionsFormatted);
    }

    private string? FormatRange(RangeAttribute ra, string fieldName)
    {
#if NET8_0_OR_GREATER
        if (ra.MinimumIsExclusive && ra.MaximumIsExclusive)
            return Format3(messageProvider.RangeMinMaxExclusive, fieldName, ra.Minimum, ra.Maximum);
        if (ra.MinimumIsExclusive)
            return Format3(messageProvider.RangeMinExclusive, fieldName, ra.Minimum, ra.Maximum);
        if (ra.MaximumIsExclusive)
            return Format3(messageProvider.RangeMaxExclusive, fieldName, ra.Minimum, ra.Maximum);
#endif
        return Format3(messageProvider.Range, fieldName, ra.Minimum, ra.Maximum);
    }

    private string? FormatStringLength(StringLengthAttribute sla, string fieldName)
    {
        if (sla.MinimumLength > 0) return Format3(
            messageProvider.StringLengthIncludingMinimum, 
            fieldName, sla.MaximumLength, sla.MinimumLength);
        
        return Format2(messageProvider.StringLength, fieldName, sla.MaximumLength);
    }

#if NET8_0_OR_GREATER
    private string? FormatLength(LengthAttribute la, string fieldName) =>
        Format3(messageProvider.Length, fieldName, la.MinimumLength, la.MaximumLength);
#endif
}