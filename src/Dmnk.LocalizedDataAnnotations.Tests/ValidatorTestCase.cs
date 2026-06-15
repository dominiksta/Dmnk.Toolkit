namespace Dmnk.LocalizedDataAnnotations.Tests;

public record ValidatorTestCase(
    object Model,
    HashSet<string> MessagesGermanContaining,
    HashSet<string> MessagesEnglishContaining,
    // System.ComponentModel.DataAnnotations internally has a limitation where attributes
    // will cache their localized messages after the first validation. switching culture
    // at runtime is therefore not supported in these cases and cannot be done in a unit test.
    // we can still test that the hack applies the correct german messages though.
    bool CanSwitchAtRuntimeWithHack = true
)
{
    public string Name => Model.GetType().Name;
    public override string ToString() => Name + "Test";
    
    public TestCaseData<ValidatorTestCase> AsTestCaseData()
    {
        var testCaseData = new TestCaseData<ValidatorTestCase>(this);
        testCaseData.SetName(ToString());
        return testCaseData;
    }
}
