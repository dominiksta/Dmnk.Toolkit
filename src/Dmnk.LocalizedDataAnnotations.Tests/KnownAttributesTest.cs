using System.ComponentModel.DataAnnotations;
using RangeAttribute = System.ComponentModel.DataAnnotations.RangeAttribute;

namespace Dmnk.LocalizedDataAnnotations.Tests;

[TestFixture]
public class KnownAttributesTest
{
    private static readonly List<Type> KnownValidationAttributes =
    [
        typeof(AllowedValuesAttribute),
        typeof(DeniedValuesAttribute),
        typeof(Base64StringAttribute),
        typeof(LengthAttribute),

        // netfx:
        typeof(CompareAttribute),
        typeof(CustomValidationAttribute),

        typeof(DataTypeAttribute),
        typeof(EnumDataTypeAttribute),
        typeof(CreditCardAttribute),
        typeof(EmailAddressAttribute),
        typeof(FileExtensionsAttribute),
        typeof(PhoneAttribute),
        typeof(UrlAttribute),

        typeof(MaxLengthAttribute),
        typeof(MinLengthAttribute),

        typeof(RangeAttribute),
        typeof(RegularExpressionAttribute),
        typeof(RequiredAttribute),

        typeof(StringLengthAttribute)
    ];

    /// <summary>
    /// If we update to a new version of .net, we should check if any new validation attributes were
    /// added to the framework and add deal with their localization accordingly.
    /// </summary>
    [Test]
    public void SanityCheck_No_New_Attributes_Were_Added()
    {
        var validationAttributeType = typeof(ValidationAttribute);
        var systemComponentModelDataAnnotations = validationAttributeType.Assembly;
        var allValidationAttributes = systemComponentModelDataAnnotations
            .GetTypes()
            .Where(t => validationAttributeType.IsAssignableFrom(t) && !t.IsAbstract)
            .ToList();

        var unknownAttributes = allValidationAttributes
            .Where(a => !KnownValidationAttributes.Contains(a))
            .ToList();
        
        using (Assert.EnterMultipleScope())
        {
            Assert.That(allValidationAttributes, Has.Count.GreaterThan(0));
            Assert.That(KnownValidationAttributes, Has.Count.GreaterThan(0));
            Assert.That(KnownValidationAttributes,
                Has.Count.LessThanOrEqualTo(allValidationAttributes.Count));
        }

        if (unknownAttributes.Count != 0)
        {
            Assert.Fail(
                $"The following ValidationAttribute subclasses were not included in " +
                $"the list of known attributes: " +
                $"{string.Join(", ", unknownAttributes.Select(a => a.FullName))}");
        }
    }
}