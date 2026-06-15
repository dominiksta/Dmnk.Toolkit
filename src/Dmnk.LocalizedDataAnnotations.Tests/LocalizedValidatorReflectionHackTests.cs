using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Resources;
using Dmnk.LocalizedDataAnnotations.Hack;

namespace Dmnk.LocalizedDataAnnotations.Tests;

[TestFixture, Parallelizable(ParallelScope.None)]
public class LocalizedValidatorReflectionHackTests
{
    private class RequiredModel
    {
        [Required] public string? Name { get; set; }
    }

    private class StringLengthModel
    {
        [StringLength(5)] public string? Name { get; set; }
    }

    private class RangeModel
    {
        [System.ComponentModel.DataAnnotations.Range(1, 10)] public int Value { get; set; } = 99;
    }

    [TearDown]
    public void TearDown()
    {
        if (LocalizedValidatorReflectionHack.IsHacked)
            LocalizedValidatorReflectionHack.UnHack();
        SetCulture("en-US");
    }

    private static void SetCulture(string cultureName)
    {
        var culture = new CultureInfo(cultureName);
        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;
        Thread.CurrentThread.CurrentCulture = culture;
        Thread.CurrentThread.CurrentUICulture = culture;
    }

    private static List<ValidationResult> Validate(object model)
    {
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(model, context, results, true);
        return results;
    }

    [Test]
    public void Hack_LocalizesRequiredAttribute()
    {
        Assert.That(
            Validate(new RequiredModel())[0].ErrorMessage, Contains.Substring("is required"),
            "Sanity check: English before hack");

        LocalizedValidatorReflectionHack.Hack(doThrow: true);

        SetCulture("de-DE");
        Assert.That(
            Validate(new RequiredModel())[0].ErrorMessage, Contains.Substring("ist erforderlich"),
            "After hack, de-DE");

        SetCulture("en-US");
        Assert.That(
            Validate(new RequiredModel())[0].ErrorMessage, Contains.Substring("is required"),
            "After hack, en-US");
    }

    [Test]
    public void UnHack_RestoresOriginalBehavior()
    {
        LocalizedValidatorReflectionHack.Hack(doThrow: true);
        LocalizedValidatorReflectionHack.UnHack(doThrow: true);

        SetCulture("de-DE");
        Assert.That(
            Validate(new RequiredModel())[0].ErrorMessage, Contains.Substring("is required"),
            "After un-hack, de-DE should still return English");
    }

    [Test]
    public void Hack_IsIdempotent()
    {
        LocalizedValidatorReflectionHack.Hack(doThrow: true);
        Assert.DoesNotThrow(() => LocalizedValidatorReflectionHack.Hack(doThrow: true));

        SetCulture("de-DE");
        Assert.That(
            Validate(new RequiredModel())[0].ErrorMessage, 
            Contains.Substring("ist erforderlich"));
    }

    [Test]
    public void UnHack_WhenNotHacked_IsNoOp()
    {
        Assert.DoesNotThrow(() => LocalizedValidatorReflectionHack.UnHack(doThrow: true));

        SetCulture("de-DE");
        Assert.That(
            Validate(new RequiredModel())[0].ErrorMessage, Contains.Substring("is required"),
            "Should remain unlocalized after no-op un-hack");
    }

    [Test]
    public void Hack_LocalizesStringLengthAttribute()
    {
        LocalizedValidatorReflectionHack.Hack(doThrow: true);
        SetCulture("de-DE");

        var results = Validate(new StringLengthModel { Name = "way too long string" });
        Assert.That(results[0].ErrorMessage, Contains.Substring("Zeichenfolge"));
    }

    [Test]
    public void Hack_LocalizesRangeAttribute()
    {
        LocalizedValidatorReflectionHack.Hack(doThrow: true);
        SetCulture("de-DE");

        var results = Validate(new RangeModel());
        Assert.That(results[0].ErrorMessage, Contains.Substring("liegen"));
    }

    [Test]
    public void Hack_WithCustomResourceManager_UsesCustomMessages()
    {
        LocalizedValidatorReflectionHack.Hack(
            doThrow: true, 
            resourceManager: new CustomResourceManager());

        var results = Validate(new RequiredModel());
        Assert.That(results[0].ErrorMessage, Contains.Substring("CUSTOM_REQUIRED"));
    }

    private class CustomResourceManager : ResourceManager
    {
        public override string? GetString(string name, CultureInfo? culture) =>
            name == "RequiredAttribute_ValidationError" ? "CUSTOM_REQUIRED: {0}" : null;
    }
}