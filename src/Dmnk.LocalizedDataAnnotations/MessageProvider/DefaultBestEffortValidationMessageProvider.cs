using Dmnk.LocalizedDataAnnotations.Properties;

namespace Dmnk.LocalizedDataAnnotations.MessageProvider;

/// <summary>
/// Uses keys based on the actual old netfx translations where possible and custom keys when
/// necessary. See the documentation site or <c>_namespace.md</c> file in the repo for details.
/// </summary>
public class DefaultBestEffortValidationMessageProvider : IDefaultValidationMessageProvider
{
    public virtual string? AllowedValues => I18nLocalizedDataAnnotations.AllowedValuesAttribute_Invalid;
    public virtual string? DeniedValues => I18nLocalizedDataAnnotations.DeniedValuesAttribute_Invalid;
    public virtual string? Base64String => I18nLocalizedDataAnnotations.Base64StringAttribute_Invalid;
    public virtual string? Length => I18nLocalizedDataAnnotations.LengthAttribute_ValidationError;

    public virtual string Fallback => I18nLocalizedDataAnnotations.ValidationAttribute_ValidationError;
    
    public virtual string? Compare => I18nLocalizedDataAnnotations.CompareAttribute_MustMatch;
    public virtual string? Custom => I18nLocalizedDataAnnotations.CustomValidationAttribute_ValidationError;
    
    public virtual string? DataTypeCreditCard => I18nLocalizedDataAnnotations.CreditCardAttribute_Invalid;
    public virtual string? DataTypeEmailAddress => I18nLocalizedDataAnnotations.EmailAddressAttribute_Invalid;
    public virtual string? DataTypeFileExtension => I18nLocalizedDataAnnotations.FileExtensionsAttribute_Invalid;
    public virtual string? DataTypePhone => I18nLocalizedDataAnnotations.PhoneAttribute_Invalid;
    public virtual string? DataTypeUrl => I18nLocalizedDataAnnotations.UrlAttribute_Invalid;

    public virtual string? MaxLength => I18nLocalizedDataAnnotations.MaxLengthAttribute_ValidationError;
    public virtual string? MinLength => I18nLocalizedDataAnnotations.MinLengthAttribute_ValidationError;
    
    public virtual string? Range => I18nLocalizedDataAnnotations.RangeAttribute_ValidationError;
    public string? RangeMaxExclusive => I18nLocalizedDataAnnotations.RangeAttribute_ValidationError_MaxExclusive;
    public string? RangeMinExclusive => I18nLocalizedDataAnnotations.RangeAttribute_ValidationError_MinExclusive;
    public string? RangeMinMaxExclusive => I18nLocalizedDataAnnotations.RangeAttribute_ValidationError_MinExclusive_MaxExclusive;

    public virtual string? RegularExpression => I18nLocalizedDataAnnotations.RegexAttribute_ValidationError;
    public virtual string? Required => I18nLocalizedDataAnnotations.RequiredAttribute_ValidationError;

    public virtual string? StringLength => I18nLocalizedDataAnnotations.StringLengthAttribute_ValidationError;
    public virtual string? StringLengthIncludingMinimum => I18nLocalizedDataAnnotations.StringLengthAttribute_ValidationErrorIncludingMinimum;
}