namespace Dmnk.LocalizedDataAnnotations.MessageProvider;

public class BestEffortDefaultValidationMessageProvider : NetFxDefaultValidationMessageProvider
{
    public override string? AllowedValues { get; }
}