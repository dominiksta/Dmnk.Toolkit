using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using Dmnk.LocalizedDataAnnotations.MessageProvider;
using RangeAttribute = System.ComponentModel.DataAnnotations.RangeAttribute;

namespace Dmnk.LocalizedDataAnnotations.Tests;

[TestFixture, Parallelizable(ParallelScope.None)]
public class LocalizedValidatorTests
{
    private LocalizedValidator _validator = null!;

    [SetUp]
    public void SetUp()
    {
        _validator = new LocalizedValidator(new DefaultBestEffortValidationMessageProvider());
    }

    [TearDown]
    public void TearDown() => SetCulture("en-US");

    private static void SetCulture(string cultureName)
    {
        var culture = new CultureInfo(cultureName);
        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;
        Thread.CurrentThread.CurrentCulture = culture;
        Thread.CurrentThread.CurrentUICulture = culture;
    }

    // --- Models ---

    private class RequiredModel { [Required] public string? Name { get; set; } }
    private class StringLengthModel { [StringLength(5)] public string? Name { get; set; } }
    private class StringLengthWithMinModel { [StringLength(10, MinimumLength = 3)] public string? Name { get; set; } }
    private class RangeModel { [Range(1, 10)] public int Value { get; set; } = 99; }
    private class RegexModel { [RegularExpression(@"^\d+$")] public string? Value { get; set; } = "abc"; }
    private class MaxLengthModel { [MaxLength(3)] public string? Value { get; set; } = "toolong"; }
    private class MinLengthModel { [MinLength(5)] public string? Value { get; set; } = "ab"; }
    private class EmailModel { [EmailAddress] public string? Value { get; set; } = "not-an-email"; }

    private class CustomMessageModel
    {
        [Required(ErrorMessage = "Custom: {0} is needed")]
        public string? Name { get; set; }
    }

    private class ValidatableModel : IValidatableObject
    {
        public string? Name { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Name == "bad")
                yield return new ValidationResult("Name cannot be 'bad'.", [nameof(Name)]);
        }
    }

    // --- Helpers ---

    private List<ValidationResult> ValidateObject(object model, bool validateAll = true)
    {
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        _validator.TryValidateObject(model, context, results, validateAll);
        return results;
    }

    private List<ValidationResult> ValidateValue(object? value, ValidationContext context, params ValidationAttribute[] attrs)
    {
        var results = new List<ValidationResult>();
        _validator.TryValidateValue(value, context, results, attrs);
        return results;
    }

    // --- TryValidateValue ---

    [Test]
    public void TryValidateValue_Required_English()
    {
        var context = new ValidationContext(new object()) { MemberName = "Name" };
        var results = ValidateValue(null, context, new RequiredAttribute());
        Assert.That(results[0].ErrorMessage, Contains.Substring("is required"));
    }

    [Test]
    public void TryValidateValue_Required_German()
    {
        SetCulture("de-DE");
        var context = new ValidationContext(new object()) { MemberName = "Name" };
        var results = ValidateValue(null, context, new RequiredAttribute());
        Assert.That(results[0].ErrorMessage, Contains.Substring("ist erforderlich"));
    }

    [Test]
    public void TryValidateValue_CustomMessage_IsNotReplaced()
    {
        SetCulture("de-DE");
        var attr = new RequiredAttribute { ErrorMessage = "Custom: {0} is needed" };
        var context = new ValidationContext(new object()) { MemberName = "Name" };
        var results = ValidateValue(null, context, attr);
        Assert.That(results[0].ErrorMessage, Does.Contain("Custom: Name is needed"));
    }

    [Test]
    public void TryValidateValue_Range_German()
    {
        SetCulture("de-DE");
        var context = new ValidationContext(new object()) { MemberName = "Value" };
        var results = ValidateValue(99, context, new RangeAttribute(1, 10));
        Assert.That(results[0].ErrorMessage, Contains.Substring("liegen"));
    }

    [Test]
    public void TryValidateValue_StringLength_German()
    {
        SetCulture("de-DE");
        var context = new ValidationContext(new object()) { MemberName = "Name" };
        var results = ValidateValue("way too long", context, new StringLengthAttribute(5));
        Assert.That(results[0].ErrorMessage, Contains.Substring("Zeichenfolge"));
    }

    [Test]
    public void TryValidateValue_PassingValidation_ReturnsTrue()
    {
        var context = new ValidationContext(new object()) { MemberName = "Name" };
        var results = new List<ValidationResult>();
        bool isValid = _validator.TryValidateValue("hello", context, results, [new RequiredAttribute()]);
        Assert.That(isValid, Is.True);
        Assert.That(results, Is.Empty);
    }

    [Test]
    public void TryValidateValue_ReturnsFalse_WhenInvalid()
    {
        var context = new ValidationContext(new object()) { MemberName = "Name" };
        var results = new List<ValidationResult>();
        bool isValid = _validator.TryValidateValue(null, context, results, [new RequiredAttribute()]);
        Assert.That(isValid, Is.False);
        Assert.That(results, Has.Count.EqualTo(1));
    }

    // --- TryValidateProperty ---

    [Test]
    public void TryValidateProperty_Required_German()
    {
        SetCulture("de-DE");
        var model = new RequiredModel();
        var context = new ValidationContext(model) { MemberName = nameof(RequiredModel.Name) };
        var results = new List<ValidationResult>();
        _validator.TryValidateProperty(null, context, results);
        Assert.That(results[0].ErrorMessage, Contains.Substring("ist erforderlich"));
    }

    // --- TryValidateObject ---

    [Test]
    public void TryValidateObject_Required_English()
    {
        var results = ValidateObject(new RequiredModel());
        Assert.That(results[0].ErrorMessage, Contains.Substring("is required"));
    }

    [Test]
    public void TryValidateObject_Required_German()
    {
        SetCulture("de-DE");
        var results = ValidateObject(new RequiredModel());
        Assert.That(results[0].ErrorMessage, Contains.Substring("ist erforderlich"));
    }

    [Test]
    public void TryValidateObject_StringLength_German()
    {
        SetCulture("de-DE");
        var results = ValidateObject(new StringLengthModel { Name = "way too long" });
        Assert.That(results[0].ErrorMessage, Contains.Substring("Zeichenfolge"));
    }

    [Test]
    public void TryValidateObject_Range_German()
    {
        SetCulture("de-DE");
        var results = ValidateObject(new RangeModel());
        Assert.That(results[0].ErrorMessage, Contains.Substring("liegen"));
    }

    [Test]
    public void TryValidateObject_Email_German()
    {
        // System.Diagnostics.Debugger.Launch();
        SetCulture("de-DE");
        var results = ValidateObject(new EmailModel());
        Assert.That(results[0].ErrorMessage, Contains.Substring("E-Mail-Adresse"));
    }

    [Test]
    public void TryValidateObject_CustomMessage_IsNotReplaced()
    {
        SetCulture("de-DE");
        var results = ValidateObject(new CustomMessageModel());
        Assert.That(results[0].ErrorMessage, Does.Contain("Custom: Name is needed"));
    }

    [Test]
    public void TryValidateObject_ValidObject_ReturnsTrue()
    {
        var context = new ValidationContext(new RequiredModel { Name = "hello" });
        var results = new List<ValidationResult>();
        bool isValid = _validator.TryValidateObject(new RequiredModel { Name = "hello" }, context, results, true);
        Assert.That(isValid, Is.True);
        Assert.That(results, Is.Empty);
    }

    [Test]
    public void TryValidateObject_ValidatesAllProperties_WhenTrue()
    {
        var results = ValidateObject(new MaxLengthModel(), validateAll: true);
        Assert.That(results, Has.Count.EqualTo(1));
    }

    [Test]
    public void TryValidateObject_OnlyValidatesRequired_WhenFalse()
    {
        // MaxLength failure should not appear when validateAllProperties = false
        var results = ValidateObject(new MaxLengthModel(), validateAll: false);
        Assert.That(results, Is.Empty);
    }

    [Test]
    public void TryValidateObject_IValidatableObject_MessagesPassThrough()
    {
        var model = new ValidatableModel { Name = "bad" };
        var results = ValidateObject(model);
        Assert.That(results[0].ErrorMessage, Is.EqualTo("Name cannot be 'bad'."));
    }

    [Test]
    public void TryValidateObject_StringLengthWithMin_German()
    {
        SetCulture("de-DE");
        var results = ValidateObject(new StringLengthWithMinModel { Name = "x" });
        Assert.That(results[0].ErrorMessage, Is.Not.Null);
        // Should contain both min and max in the message
        Assert.That(results[0].ErrorMessage, Does.Contain("3").And.Contain("10"));
    }
}
