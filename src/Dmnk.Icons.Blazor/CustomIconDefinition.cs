using Dmnk.Icons.Core;
using Microsoft.AspNetCore.Components;

namespace Dmnk.Icons.Blazor;

public abstract class CustomIconDefinition(string technicalName) : IconDefinition(technicalName)
{
    public abstract MarkupString ToMarkup();
}