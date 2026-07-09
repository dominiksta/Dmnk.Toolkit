namespace Dmnk.Blazor.InteractiveTests.Api;

/// <summary>
/// Injected into every interactive test, as well as the test bed.
/// </summary>
public sealed record InteractiveTestContext
{
    /// <summary>
    /// The test project name. This is specifically useful in testbeds, which need to
    /// have a HeadContent that loads the test projects styles.
    /// </summary>
    public required string TestProjectName { get; init; } 
}