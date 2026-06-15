using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Dmnk.LocalizedDataAnnotations.MessageProvider;

namespace Dmnk.LocalizedDataAnnotations.Tests;

/// <summary>
/// Verifies that <see cref="LocalizedValidator"/> produces identical English error messages
/// to the builtin <see cref="Validator"/> for all known attribute types.
/// </summary>
[TestFixture, Parallelizable(ParallelScope.None), SetCulture("en")]
public class LocalizedValidatorCompatibilityTests
{
    private LocalizedValidator _validator = null!;

    [SetUp]
    public void SetUp()
    {
        _validator = new LocalizedValidator(new DefaultBestEffortValidationMessageProvider());
    }

    private static List<ValidationResult> ValidateWithBuiltin(object model)
    {
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(model, context, results, true);
        return results;
    }

    private List<ValidationResult> ValidateWithLocalized(object model)
    {
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        _validator.TryValidateObject(model, context, results, true);
        return results;
    }

    [Test]
    [TestCaseSource(typeof(ValidatorTestCases), nameof(ValidatorTestCases.All))]
    public void MatchesBuiltin_English(ValidatorTestCase tc)
    {
        var builtin = ValidateWithBuiltin(tc.Model);
        var localized = ValidateWithLocalized(tc.Model);

        Assert.That(localized, Has.Count.EqualTo(builtin.Count),
            "Result count differs from builtin Validator.");

        foreach (var expected in builtin)
        {
            Assert.That(
                localized.Any(r => r.ErrorMessage == expected.ErrorMessage),
                Is.True,
                $"LocalizedValidator did not produce message: \"{expected.ErrorMessage}\".\n" +
                $" Got messages: " +
                $"{string.Join(", ", localized.Select(r => $"\"{r.ErrorMessage}\""))}");
        }
    }
}
