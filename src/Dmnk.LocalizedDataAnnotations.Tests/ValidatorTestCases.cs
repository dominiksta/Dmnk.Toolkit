using System.ComponentModel.DataAnnotations;
using RangeAttribute = System.ComponentModel.DataAnnotations.RangeAttribute;

namespace Dmnk.LocalizedDataAnnotations.Tests;

public static class ValidatorTestCases
{
    public class RequiredModel
    {
        [Required] public string? Name { get; set; }
    }
    public static ValidatorTestCase RequiredTest => new(
        new RequiredModel(),
        MessagesGermanContaining: ["ist erforderlich"],
        MessagesEnglishContaining: ["is required"]);

    public class StringLengthModel
    {
        [StringLength(5)] public string? Name { get; set; } = "toolongstring";
    }
    public static ValidatorTestCase StringLengthTest => new(
        new StringLengthModel(),
        MessagesGermanContaining: ["maximalen Länge"],
        MessagesEnglishContaining: ["maximum length"]);

    public class StringLengthWithMinModel
    {
        [StringLength(10, MinimumLength = 3)] public string? Name { get; set; } = "x";
    }
    public static ValidatorTestCase StringLengthWithMinTest => new(
        new StringLengthWithMinModel(),
        MessagesGermanContaining: ["Mindestlänge", "Höchstlänge"],
        MessagesEnglishContaining: ["minimum length", "maximum length"]);

    public class RangeModel
    {
        [Range(1, 10)] public int Value { get; set; } = 99;
    }
    public static ValidatorTestCase RangeTest => new(
        new RangeModel(),
        MessagesGermanContaining: ["liegen"],
        MessagesEnglishContaining: ["must be between"]);

    public class RangeMaxExclusiveModel
    {
        [Range(1, 10, MaximumIsExclusive = true)] public int Value { get; set; } = 10;
    }
    public static ValidatorTestCase RangeMaxExclusiveTest => new(
        new RangeMaxExclusiveModel(),
        MessagesGermanContaining: ["exklusive"],
        MessagesEnglishContaining: ["exclusive"]);

    public class RangeMinExclusiveModel
    {
        [Range(1, 10, MinimumIsExclusive = true)] public int Value { get; set; } = 1;
    }
    public static ValidatorTestCase RangeMinExclusiveTest => new(
        new RangeMinExclusiveModel(),
        MessagesGermanContaining: ["exklusive"],
        MessagesEnglishContaining: ["exclusive"]);

    public class RangeMinMaxExclusiveModel
    {
        [Range(1, 10, MinimumIsExclusive = true, MaximumIsExclusive = true)]
        public int Value { get; set; } = 1;
    }
    public static ValidatorTestCase RangeMinMaxExclusiveTest => new(
        new RangeMinMaxExclusiveModel(),
        MessagesGermanContaining: ["exklusive"],
        MessagesEnglishContaining: ["exclusive"]);

    public class RegularExpressionModel
    {
        [RegularExpression(@"^\d+$")] public string? Value { get; set; } = "abc";
    }
    public static ValidatorTestCase RegularExpressionTest => new(
        new RegularExpressionModel(),
        MessagesGermanContaining: ["regulären Ausdruck"],
        MessagesEnglishContaining: ["regular expression"]);

    public class MaxLengthModel
    {
        [MaxLength(3)] public string? Value { get; set; } = "toolong";
    }
    public static ValidatorTestCase MaxLengthTest => new(
        new MaxLengthModel(),
        MessagesGermanContaining: ["maximalen Länge"],
        MessagesEnglishContaining: ["maximum length"]);

    public class MinLengthModel
    {
        [MinLength(5)] public string? Value { get; set; } = "ab";
    }
    public static ValidatorTestCase MinLengthTest => new(
        new MinLengthModel(),
        MessagesGermanContaining: ["minimalen Länge"],
        MessagesEnglishContaining: ["minimum length"],
        CanSwitchAtRuntimeWithHack: false);

    public class LengthModel
    {
        [Length(3, 10)] public string? Value { get; set; } = "x";
    }
    public static ValidatorTestCase LengthTest => new(
        new LengthModel(),
        MessagesGermanContaining: ["Maximallänge"],
        MessagesEnglishContaining: ["collection type"],
        CanSwitchAtRuntimeWithHack: false);

    public class EmailAddressModel
    {
        [EmailAddress] public string? Value { get; set; } = "not-an-email";
    }
    public static ValidatorTestCase EmailAddressTest => new(
        new EmailAddressModel(),
        MessagesGermanContaining: ["E-Mail-Adresse"],
        MessagesEnglishContaining: ["e-mail address"],
        CanSwitchAtRuntimeWithHack: false);

    public class CreditCardModel
    {
        // Clearly invalid - not a digit string, so the Luhn check is never even reached.
        [CreditCard] public string? Value { get; set; } = "invalid";
    }
    public static ValidatorTestCase CreditCardTest => new(
        new CreditCardModel(),
        MessagesGermanContaining: ["Kreditkartennummer"],
        MessagesEnglishContaining: ["credit card number"],
        CanSwitchAtRuntimeWithHack: false);

    public class PhoneModel
    {
        [Phone] public string? Value { get; set; } = "!@#$%";
    }
    public static ValidatorTestCase PhoneTest => new(
        new PhoneModel(),
        MessagesGermanContaining: ["Telefonnummer"],
        MessagesEnglishContaining: ["phone number"],
        CanSwitchAtRuntimeWithHack: false);

    public class UrlModel
    {
        [Url] public string? Value { get; set; } = "not-a-url";
    }
    public static ValidatorTestCase UrlTest => new(
        new UrlModel(),
        MessagesGermanContaining: ["HTTP"],
        MessagesEnglishContaining: ["http"],
        CanSwitchAtRuntimeWithHack: false);

    public class FileExtensionsModel
    {
        [FileExtensions(Extensions = "jpg,png")] public string? FileName { get; set; } = "file.exe";
    }
    public static ValidatorTestCase FileExtensionsTest => new(
        new FileExtensionsModel(),
        MessagesGermanContaining: ["Erweiterungen"],
        MessagesEnglishContaining: ["extensions"],
        CanSwitchAtRuntimeWithHack: false);

    public class CompareModel
    {
        public string? Password { get; set; } = "abc";
        [Compare(nameof(Password))] public string? ConfirmPassword { get; set; } = "xyz";
    }
    public static ValidatorTestCase CompareTest => new(
        new CompareModel(),
        MessagesGermanContaining: ["stimmen nicht überein"],
        MessagesEnglishContaining: ["do not match"],
        CanSwitchAtRuntimeWithHack: false);

    public class AllowedValuesModel
    {
        [AllowedValues("a", "b")] public string? Value { get; set; } = "invalid";
    }
    public static ValidatorTestCase AllowedValuesTest => new(
        new AllowedValuesModel(),
        MessagesGermanContaining: ["eingeschlossenen Werte"],
        MessagesEnglishContaining: ["AllowedValuesAttribute"],
        CanSwitchAtRuntimeWithHack: false);

    public class DeniedValuesModel
    {
        [DeniedValues("bad")] public string? Value { get; set; } = "bad";
    }
    public static ValidatorTestCase DeniedValuesTest => new(
        new DeniedValuesModel(),
        MessagesGermanContaining: ["ausgeschlossenen Wert"],
        MessagesEnglishContaining: ["DeniedValuesAttribute"],
        CanSwitchAtRuntimeWithHack: false);

    public class Base64StringModel
    {
        // Invalid chars and length not divisible by 4.
        [Base64String] public string? Value { get; set; } = "not-base64!@#";
    }
    public static ValidatorTestCase Base64StringTest => new(
        new Base64StringModel(),
        MessagesGermanContaining: ["Base64"],
        MessagesEnglishContaining: ["Base64"]);

    public class EnumDataTypeModel
    {
        // DayOfWeek is 0-6; 999 is not defined.
        [EnumDataType(typeof(DayOfWeek))] public int Value { get; set; } = 999;
    }
    public static ValidatorTestCase EnumDataTypeTest => new(
        new EnumDataTypeModel(),
        MessagesGermanContaining: ["ist ungültig"],
        MessagesEnglishContaining: ["is invalid"]);


    // -- Multi-property models ------------------------------------------------

    public class RegistrationModel
    {
        [Required] public string? Name { get; set; }
        [EmailAddress] public string? Email { get; set; } = "not-an-email";
        [Range(1, 120)] public int Age { get; set; } = -1;
    }
    public static ValidatorTestCase RegistrationTest => new(
        new RegistrationModel(),
        MessagesGermanContaining: ["ist erforderlich", "E-Mail-Adresse", "liegen"],
        MessagesEnglishContaining: ["is required", "e-mail address", "must be between"],
        CanSwitchAtRuntimeWithHack: false);

    public class MultipleRequiredModel
    {
        [Required] public string? FirstName { get; set; }
        [Required] public string? LastName { get; set; }
    }
    public static ValidatorTestCase MultipleRequiredTest => new(
        new MultipleRequiredModel(),
        MessagesGermanContaining: ["ist erforderlich"],
        MessagesEnglishContaining: ["is required"]);

    public class MultipleAttributesSamePropertyModel
    {
        // Both [Required] and [StringLength] on the same property; both should fire
        // because the value is null (Required) and would also violate StringLength.
        [Required, StringLength(5, MinimumLength = 2)] public string? Code { get; set; }
    }
    public static ValidatorTestCase MultipleAttributesSamePropertyTest => new(
        new MultipleAttributesSamePropertyModel(),
        MessagesGermanContaining: ["ist erforderlich"],
        MessagesEnglishContaining: ["is required"]);

    // -- validateAllProperties = false ----------------------------------------

    public class MixedRequiredAndStringLengthModel
    {
        [Required] public string? Name { get; set; }
        // Code is present but too long — should NOT be validated when
        // validateAllProperties = false, because it is not [Required].
        [StringLength(5)] public string? Code { get; set; } = "toolongstring";
    }
    public static ValidatorTestCase ValidateAllPropertiesFalseTest => new(
        new MixedRequiredAndStringLengthModel(),
        MessagesGermanContaining: ["ist erforderlich"],
        MessagesEnglishContaining: ["is required"],
        ValidateAllProperties: false);

    // -- Happy path (no errors expected) --------------------------------------

    public class ValidPersonModel
    {
        [Required] public string? Name { get; set; } = "John";
        [EmailAddress] public string? Email { get; set; } = "john@example.com";
        [Range(1, 120)] public int Age { get; set; } = 30;
    }
    public static ValidatorTestCase ValidModelTest => new(
        new ValidPersonModel(),
        MessagesGermanContaining: [],
        MessagesEnglishContaining: []);

    public class ValidRangesModel
    {
        [Range(0, 100)] public int Score { get; set; } = 50;
        [StringLength(50, MinimumLength = 1)] public string? Title { get; set; } = "Hello";
    }
    public static ValidatorTestCase ValidModelWithRangeTest => new(
        new ValidRangesModel(),
        MessagesGermanContaining: [],
        MessagesEnglishContaining: []);

    // -- IValidatableObject ---------------------------------------------------

    public class FailingValidatableModel : IValidatableObject
    {
        public string? Value { get; set; } = "trigger-error";

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Value == "trigger-error")
                yield return new ValidationResult(
                    "custom-object-error",
                    [nameof(Value)]);
        }
    }
    public static ValidatorTestCase IValidatableObjectFailingTest => new(
        new FailingValidatableModel(),
        MessagesGermanContaining: ["custom-object-error"],
        MessagesEnglishContaining: ["custom-object-error"]);

    public class PassingValidatableModel : IValidatableObject
    {
        public string? Value { get; set; } = "ok";

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            yield break;
        }
    }
    public static ValidatorTestCase IValidatableObjectValidTest => new(
        new PassingValidatableModel(),
        MessagesGermanContaining: [],
        MessagesEnglishContaining: []);

    // IValidatableObject combined with a failing [Required] attribute
    public class ValidatableWithAttributeModel : IValidatableObject
    {
        [Range(2, 4)] public int Number { get; set; } = 0;
        
        public string? Tag { get; set; } = "missing-prefix";

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            yield return new ValidationResult("Something", [nameof(Tag)]);
        }
    }
    public static ValidatorTestCase IValidatableObjectWithAttributeTest => new(
        new ValidatableWithAttributeModel(),
        MessagesGermanContaining: ["liegen"],
        MessagesEnglishContaining: ["between"]);

    // -- Custom ValidationAttribute subclass ----------------------------------

    public class MustStartWithUpperCaseAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext ctx)
        {
            if (value is string s && s.Length > 0 && !char.IsUpper(s[0]))
                return new ValidationResult(
                    $"The field {ctx.DisplayName} must start with an uppercase letter.",
                    [ctx.MemberName!]);
            return ValidationResult.Success;
        }
    }
    public class CustomAttributeModel
    {
        [MustStartWithUpperCase] public string? Name { get; set; } = "lowercase";
    }
    public static ValidatorTestCase CustomAttributeTest => new(
        new CustomAttributeModel(),
        MessagesGermanContaining: ["uppercase letter"],
        MessagesEnglishContaining: ["uppercase letter"]);

    // Custom attribute that uses ErrorMessage (format string) via FormatErrorMessage
    public class MustBePositiveAttribute : ValidationAttribute
    {
        public MustBePositiveAttribute() { ErrorMessage = "The field {0} must be a positive number."; }

        protected override ValidationResult? IsValid(object? value, ValidationContext ctx)
        {
            if (value is int i && i <= 0)
                return new ValidationResult(FormatErrorMessage(ctx.DisplayName), [ctx.MemberName!]);
            return ValidationResult.Success;
        }
    }
    public class CustomAttributeWithFormatModel
    {
        [MustBePositive] public int Value { get; set; } = -5;
    }
    public static ValidatorTestCase CustomAttributeWithFormatTest => new(
        new CustomAttributeWithFormatModel(),
        MessagesGermanContaining: ["must be a positive number"],
        MessagesEnglishContaining: ["must be a positive number"]);

    // -- CustomValidationAttribute --------------------------------------------

    public static class SkuValidator
    {
        public static ValidationResult? ValidateSku(string? value, ValidationContext ctx)
        {
            if (value == null || !value.StartsWith("SKU-"))
                return new ValidationResult(
                    "SKU must start with 'SKU-'.",
                    [ctx.MemberName!]);
            return ValidationResult.Success;
        }
    }
    public class ProductModel
    {
        [CustomValidation(typeof(SkuValidator), nameof(SkuValidator.ValidateSku))]
        public string? Sku { get; set; } = "INVALID-123";
    }
    public static ValidatorTestCase CustomValidationAttributeTest => new(
        new ProductModel(),
        MessagesGermanContaining: ["SKU-"],
        MessagesEnglishContaining: ["SKU-"]);

    public class ValidProductModel
    {
        [CustomValidation(typeof(SkuValidator), nameof(SkuValidator.ValidateSku))]
        public string? Sku { get; set; } = "SKU-001";
    }
    public static ValidatorTestCase CustomValidationAttributeValidTest => new(
        new ValidProductModel(),
        MessagesGermanContaining: [],
        MessagesEnglishContaining: []);

    public static IEnumerable<TestCaseData> All
    {
        get
        {
            yield return RequiredTest.AsTestCaseData();
            yield return StringLengthTest.AsTestCaseData();
            yield return StringLengthWithMinTest.AsTestCaseData();
            yield return RangeTest.AsTestCaseData();
            yield return RangeMaxExclusiveTest.AsTestCaseData();
            yield return RangeMinExclusiveTest.AsTestCaseData();
            yield return RangeMinMaxExclusiveTest.AsTestCaseData();
            yield return RegularExpressionTest.AsTestCaseData();
            yield return MaxLengthTest.AsTestCaseData();
            yield return MinLengthTest.AsTestCaseData();
            yield return LengthTest.AsTestCaseData();
            yield return EmailAddressTest.AsTestCaseData();
            yield return CreditCardTest.AsTestCaseData();
            yield return PhoneTest.AsTestCaseData();
            yield return UrlTest.AsTestCaseData();
            yield return FileExtensionsTest.AsTestCaseData();
            yield return CompareTest.AsTestCaseData();
            yield return AllowedValuesTest.AsTestCaseData();
            yield return DeniedValuesTest.AsTestCaseData();
            yield return Base64StringTest.AsTestCaseData();
            yield return EnumDataTypeTest.AsTestCaseData();
            // multi-property
            yield return RegistrationTest.AsTestCaseData();
            yield return MultipleRequiredTest.AsTestCaseData();
            yield return MultipleAttributesSamePropertyTest.AsTestCaseData();
            // validateAllProperties = false
            yield return ValidateAllPropertiesFalseTest.AsTestCaseData();
            // happy path
            yield return ValidModelTest.AsTestCaseData();
            yield return ValidModelWithRangeTest.AsTestCaseData();
            // IValidatableObject
            yield return IValidatableObjectFailingTest.AsTestCaseData();
            yield return IValidatableObjectValidTest.AsTestCaseData();
            yield return IValidatableObjectWithAttributeTest.AsTestCaseData();
            // custom ValidationAttribute subclass
            yield return CustomAttributeTest.AsTestCaseData();
            yield return CustomAttributeWithFormatTest.AsTestCaseData();
            // CustomValidationAttribute
            yield return CustomValidationAttributeTest.AsTestCaseData();
            yield return CustomValidationAttributeValidTest.AsTestCaseData();
        }
    }
}
