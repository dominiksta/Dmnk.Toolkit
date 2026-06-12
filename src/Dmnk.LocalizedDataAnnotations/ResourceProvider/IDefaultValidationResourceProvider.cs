namespace Dmnk.LocalizedDataAnnotations.ResourceProvider;

public interface IDefaultValidationResourceProvider
{
    string Required { get; }
    string StringLength { get; }
    string MinLength { get; }
    string MaxLength { get; }
    string Range { get; }
    string EmailAddress { get; }
    string Compare { get; }
    string RegularExpression { get; }
}
