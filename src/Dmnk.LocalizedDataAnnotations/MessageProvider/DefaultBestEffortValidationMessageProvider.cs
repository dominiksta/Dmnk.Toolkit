using Dmnk.LocalizedDataAnnotations.Properties;

namespace Dmnk.LocalizedDataAnnotations.MessageProvider;

/// <summary>
/// Uses keys based on the actual old netfx translations where possible and custom keys when
/// necessary. See the documentation site or <c>_namespace.md</c> file in the repo for details.
/// </summary>
internal sealed class DefaultBestEffortValidationMessageProvider : IDefaultValidationMessageProvider
{
    public string? AllowedValues => I18nLocalizedDataAnnotations.AllowedValuesAttribute_Invalid;
    public string? DeniedValues => I18nLocalizedDataAnnotations.DeniedValuesAttribute_Invalid;
    public string? Base64String => I18nLocalizedDataAnnotations.Base64StringAttribute_Invalid;
    public string? Length => I18nLocalizedDataAnnotations.LengthAttribute_ValidationError;

    public string Fallback => I18nLocalizedDataAnnotations.ValidationAttribute_ValidationError;
    
    public string? Compare => I18nLocalizedDataAnnotations.CompareAttribute_MustMatch;
    public string? Custom => I18nLocalizedDataAnnotations.CustomValidationAttribute_ValidationError;
    
    public string? DataTypeCreditCard => I18nLocalizedDataAnnotations.CreditCardAttribute_Invalid;
    public string? DataTypeEmailAddress => I18nLocalizedDataAnnotations.EmailAddressAttribute_Invalid;
    public string? DataTypeFileExtension => I18nLocalizedDataAnnotations.FileExtensionsAttribute_Invalid;
    public string? DataTypePhone => I18nLocalizedDataAnnotations.PhoneAttribute_Invalid;
    public string? DataTypeUrl => I18nLocalizedDataAnnotations.UrlAttribute_Invalid;

    public string? MaxLength => I18nLocalizedDataAnnotations.MaxLengthAttribute_ValidationError;
    public string? MinLength => I18nLocalizedDataAnnotations.MinLengthAttribute_ValidationError;
    
    public string? Range => I18nLocalizedDataAnnotations.RangeAttribute_ValidationError;
    public string? RangeMaxExclusive => I18nLocalizedDataAnnotations.RangeAttribute_ValidationError_MaxExclusive;
    public string? RangeMinExclusive => I18nLocalizedDataAnnotations.RangeAttribute_ValidationError_MinExclusive;
    public string? RangeMinMaxExclusive => I18nLocalizedDataAnnotations.RangeAttribute_ValidationError_MinExclusive_MaxExclusive;

    public string? RegularExpression => I18nLocalizedDataAnnotations.RegexAttribute_ValidationError;
    public string? Required => I18nLocalizedDataAnnotations.RequiredAttribute_ValidationError;

    public string? StringLength => I18nLocalizedDataAnnotations.StringLengthAttribute_ValidationError;
    public string? StringLengthIncludingMinimum => I18nLocalizedDataAnnotations.StringLengthAttribute_ValidationErrorIncludingMinimum;
}