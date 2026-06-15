using System.ComponentModel.DataAnnotations;

namespace Dmnk.LocalizedDataAnnotations.MessageProvider;

/// <summary>
/// Defines the set of messages that the default <see cref="ValidationAttribute"/>s
/// in <see cref="System.ComponentModel.DataAnnotations"/> use.
/// </summary>
public interface IDefaultValidationMessageProvider
{
    // The easiest way to find these fields is to just ask the ide to find references of the
    // internal class System.SR in System.ComponentModel.DataAnnotations and/or find subclasses
    // of ValidationAttribute.
    
    // Available since net8
    // ======================================================================
    
    /// <summary>
    /// net8+: AllowedValuesAttribute
    /// <p> key: AllowedValuesAttribute_Invalid </p>
    /// </summary>
    public string? AllowedValues { get; }
    /// <summary>
    /// net8+: DeniedValuesAttribute
    /// <p> key: DeniedValuesAttribute_Invalid </p>
    /// </summary>
    public string? DeniedValues { get; }
    /// <summary>
    /// net8+: Base64StringAttribute
    /// <p> key: Base64StringAttribute_Invalid </p>
    /// </summary>
    public string? Base64String { get; }
    /// <summary>
    /// net8+: LengthAttribute
    /// <p> key: LengthAttribute_ValidationError </p>
    /// </summary>
    public string? Length { get; }
    
    // Available since netfx
    // ======================================================================
    
    /// <summary>
    /// All other messages are optional - this one is not since it is used as a fallback.
    /// <p> Suggested key: ValidationAttribute_ValidationError </p>
    /// </summary>
    public string Fallback { get; }
    
    /// <summary> key: CompareAttribute_MustMatch </summary>
    public string? Compare { get; }
    /// <summary> key: CustomValidationAttribute_ValidationError </summary>
    public string? Custom { get; }
    
    /// <summary> key: CreditCardAttribute_Invalid </summary>
    public string? DataTypeCreditCard { get; }
    /// <summary> key: EmailAddressAttribute_Invalid </summary>
    public string? DataTypeEmailAddress { get; }
    /// <summary> key: FileExtensionsAttribute_Invalid </summary>
    public string? DataTypeFileExtension { get; }
    /// <summary> key: PhoneAttribute_Invalid </summary>
    public string? DataTypePhone { get; }
    /// <summary> key: UrlAttribute_Invalid </summary>
    public string? DataTypeUrl { get; }
    
    /// <summary> key: MaxLengthAttribute_ValidationError </summary>
    public string? MaxLength { get; }
    /// <summary> key: MinLengthAttribute_ValidationError </summary>
    public string? MinLength { get; }
    
    /// <summary>
    /// Note that technically, net10 specifies extra i18n fields for min/max exclusive settings.
    /// To remain compatible with the old netfx translations, we instead add/subtract before
    /// showing the message as necessary.
    /// <p> key: RangeAttribute_ValidationError </p>
    /// </summary>
    public string? Range { get; }
    
    /// <summary> key: RegexAttribute_ValidationError </summary>
    public string? RegularExpression { get; }
    /// <summary> key: RequiredAttribute_ValidationError </summary>
    public string? Required { get; }
    
    /// <summary> key: StringLengthAttribute_ValidationError </summary>
    public string? StringLength { get; }
    /// <summary> key: StringLengthAttribute_ValidationErrorIncludingMinimum </summary>
    public string? StringLengthIncludingMinimum { get; }
}