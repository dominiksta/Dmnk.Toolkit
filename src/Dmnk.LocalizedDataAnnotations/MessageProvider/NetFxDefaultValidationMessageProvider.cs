using Dmnk.LocalizedDataAnnotations.Properties;

namespace Dmnk.LocalizedDataAnnotations.MessageProvider;

/// <summary>
/// Sets only the values that were present in the net48/netstandard2.0 version of
/// <see cref="System.ComponentModel.DataAnnotations"/>.
/// </summary>
public class NetFxDefaultValidationMessageProvider : IDefaultValidationMessageProvider
{
    public virtual string? AllowedValues => null;
    public virtual string? DeniedValues => null;
    public virtual string? Base64String => null;
    public virtual string? Length => null;

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
    
    public virtual string? RegularExpression => I18nLocalizedDataAnnotations.RegexAttribute_ValidationError;
    public virtual string? Required => I18nLocalizedDataAnnotations.RequiredAttribute_ValidationError;

    public virtual string? StringLength => I18nLocalizedDataAnnotations.StringLengthAttribute_ValidationError;
    public virtual string? StringLengthIncludingMinimum => I18nLocalizedDataAnnotations.StringLengthAttribute_ValidationErrorIncludingMinimum;
}