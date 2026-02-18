using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Dmnk.Blazor.Mvvm.SourceGen;

/// <summary>
/// Generates automatic ViewModel-to-View registrations for types marked with [ViewModelFor]
/// attribute.
/// </summary>
/// <example>
/// MyViewModel.cs:
/// <code>
/// [ViewModelFor(typeof(MyView))]
/// public class MyViewModel : INotifyPropertyChanged { ... }
/// </code>
///
/// MyView.razor:
/// <code lang="razor">
/// @inherits MvvmComponentBase&lt;MyViewModel&gt;
/// ...
/// </code>
///
/// Program.cs:
/// <code>
/// // ...
/// ServiceProvider provider = services.BuildServiceProvider();
/// 
/// provider.GetRequiredService&lt;IViewModelRegistry&gt;().AutoRegisterFromSourceGen();
/// </code>
///
/// MyDynamicView.razor:
/// <code lang="razor">
/// &lt;RegisteredViewFor Vm="myDynamicViewModel" /&gt;
/// </code>
/// </example>
[Generator]
public class ViewModelRegistryGenerator : IIncrementalGenerator
{
    private static readonly DiagnosticDescriptor DuplicateViewRegistrationRule = new(
        id: "DMVB001",
        title: "Multiple ViewModels registered for the same View",
        messageFormat: """
                       The view type '{0}' is registered with multiple ViewModels: {1}. 
                       Only the last registration will be used at runtime.
                       """.Replace("\n", " ").Trim(),
        category: "Dmnk.Blazor.Mvvm.SourceGen",
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true);

    private static readonly DiagnosticDescriptor ViewTypeNotComponentRule = new(
        id: "DMVB002",
        title: "View type does not inherit from ComponentBase",
        messageFormat: """
                       The view type '{0}' specified in [ViewModelFor] attribute on '{1}' does 
                       not inherit from ComponentBase. This may cause runtime errors.
                       """.Replace("\n", " ").Trim(),
        category: "Dmnk.Blazor.Mvvm.SourceGen",
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true);

    /// <summary> <inheritdoc/> </summary>
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // Find all classes with the ViewFor attribute
        var viewModelDeclarations = context.SyntaxProvider
            .CreateSyntaxProvider(
                predicate: static (s, _) => 
                    s is ClassDeclarationSyntax { AttributeLists.Count: > 0 },
                transform: static (ctx, ct) => GetSemanticTargetForGeneration(ctx))
            .Where(static m => m.HasValue);

        // Combine with the compilation
        var compilationAndViewModels = context.CompilationProvider.Combine(viewModelDeclarations.Collect());

        // Generate the registration code
        context.RegisterSourceOutput(compilationAndViewModels,
            static (spc, source) => Execute(source.Left, source.Right!, spc));
    }

    private static ViewModelInfoWithDiagnostics? GetSemanticTargetForGeneration(GeneratorSyntaxContext context)
    {
        var classDecl = (ClassDeclarationSyntax)context.Node;
        var classSymbol = context.SemanticModel.GetDeclaredSymbol(classDecl) as INamedTypeSymbol;

        if (classSymbol == null)
            return null;

        // Look for the ViewFor attribute
        foreach (var attribute in classSymbol.GetAttributes())
        {
            if (attribute.AttributeClass?.Name != "ViewModelForAttribute" &&
                attribute.AttributeClass?.Name != "ViewModelFor")
                continue;

            // Get the view type from the attribute constructor argument
            if (attribute.ConstructorArguments.Length > 0)
            {
                var viewTypeArg = attribute.ConstructorArguments[0];
                if (viewTypeArg.Value is INamedTypeSymbol viewTypeSymbol)
                {
                    var diagnostics = new List<DiagnosticInfo>();
                    var originalViewTypeName = viewTypeSymbol.Name;
                    var viewTypeName = 
                        viewTypeSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
                    var viewModelTypeName = classSymbol.ToDisplayString(
                        SymbolDisplayFormat.FullyQualifiedFormat);
                    
                    // DMVB003: Check if view type inherits from ComponentBase
                    if (!InheritsFromComponentBase(viewTypeSymbol, context.SemanticModel.Compilation))
                    {
                        var location = attribute.ApplicationSyntaxReference?.GetSyntax().GetLocation() 
                            ?? classDecl.GetLocation();
                        diagnostics.Add(new DiagnosticInfo(
                            ViewTypeNotComponentRule,
                            location,
                            originalViewTypeName,
                            viewModelTypeName));
                    }

                    // DMVB001: If the view type has no namespace, check if it's actually in the same namespace
                    if (!viewTypeName.Contains("."))
                    {
                        var viewModelNamespace = GetNamespace(classSymbol);
                        
                        // Assume same namespace if we couldn't determine otherwise
                        viewTypeName = string.IsNullOrEmpty(viewModelNamespace)
                            ? viewTypeName
                            : $"{viewModelNamespace}.{viewTypeName}";
                    }

                    return new ViewModelInfoWithDiagnostics(
                        ViewModelType: viewModelTypeName,
                        ViewType: viewTypeName,
                        Diagnostics: diagnostics.ToArray()
                    );
                }
            }
        }

        return null;
    }

    private static void Execute(
        Compilation compilation, 
        ImmutableArray<ViewModelInfoWithDiagnostics?> viewModels, 
        SourceProductionContext context)
    {
        if (viewModels.IsDefaultOrEmpty)
            return;

        var validViewModels = viewModels
            .Where(vm => vm.HasValue)
            .Select(vm => vm!.Value)
            .ToList();

        if (validViewModels.Count == 0)
            return;

        // Report all diagnostics
        foreach (var vm in validViewModels)
        {
            foreach (var diagnostic in vm.Diagnostics)
            {
                context.ReportDiagnostic(Diagnostic.Create(
                    diagnostic.Descriptor,
                    diagnostic.Location,
                    diagnostic.MessageArgs));
            }
        }

        // DMVB002: Check for duplicate View registrations
        var viewGroups = validViewModels
            .GroupBy(vm => vm.ViewType)
            .Where(g => g.Count() > 1)
            .ToList();

        foreach (var group in viewGroups)
        {
            var viewModelsForView = string.Join(", ", group.Select(vm => vm.ViewModelType));
            // Report diagnostic for duplicates (use Location.None since this is a cross-file issue)
            context.ReportDiagnostic(Diagnostic.Create(
                DuplicateViewRegistrationRule,
                Location.None,
                group.Key,
                viewModelsForView));
        }

        var distinctViewModels = validViewModels
            .Select(vm => new ViewModelInfo(vm.ViewModelType, vm.ViewType))
            .Distinct()
            .OrderBy(vm => vm.ViewModelType)
            .ToList();

        var rootNamespace = GetCurrentProjectRootNamespace(compilation);
        var source = GenerateRegistrationClass(
            distinctViewModels, compilation.AssemblyName ?? "Assembly", rootNamespace);
        const string fileName = 
            "ViewModelRegistryExtensions_CHECK_ViewModelFor_ANNOTATIONS_IF_COMPILATION_FAILS.g.cs";
        context.AddSource(fileName, SourceText.From(source, Encoding.UTF8));
    }

    private static string GenerateRegistrationClass(
        List<ViewModelInfo> viewModels,
        string assemblyName,
        string rootNamespace)
    {
        var sb = new StringBuilder();
        sb.AppendLine("// <auto-generated/>");
        sb.AppendLine("#nullable enable");
        sb.AppendLine();
        sb.AppendLine($"namespace {rootNamespace};");
        sb.AppendLine();
        sb.AppendLine("/// <summary>");
        sb.AppendLine("/// Auto-generated extension methods for registering ViewModels with their Views.");
        sb.AppendLine("/// </summary>");
        sb.AppendLine("public static class SourceGeneratedViewModelRegistrations");
        sb.AppendLine("{");
        sb.AppendLine("    /// <summary>");
        sb.AppendLine($"    /// Registers all ViewModels from {assemblyName} that are marked with [ViewModelFor] attribute.");
        sb.AppendLine("    /// </summary>");
        sb.AppendLine("    public static void Register(Dmnk.Blazor.Mvvm.IViewModelRegistry registry)");
        sb.AppendLine("    {");

        foreach (var vm in viewModels)
        {
            sb.AppendLine($"        registry.RegisterDynamic(typeof({vm.ViewModelType}), typeof({vm.ViewType}));");
        }

        sb.AppendLine("    }");
        sb.AppendLine("}");

        return sb.ToString();
    }

    private static string GetNamespace(INamedTypeSymbol symbol)
    {
        var ns = symbol.ContainingNamespace;
        if (ns == null || ns.IsGlobalNamespace)
            return string.Empty;

        var parts = new List<string>();
        while (ns != null && !ns.IsGlobalNamespace)
        {
            parts.Insert(0, ns.Name);
            ns = ns.ContainingNamespace;
        }
        return string.Join(".", parts);
    }

    private static string GetCurrentProjectRootNamespace(Compilation compilation)
    {
        // Use the assembly name as the root namespace
        // This is the default behavior in .NET projects where RootNamespace = AssemblyName
        var assemblyName = compilation.AssemblyName;
        
        if (!string.IsNullOrEmpty(assemblyName))
        {
            return assemblyName!;
        }
        
        // Fallback to "Dmnk.Blazor.Mvvm" if we can't determine it
        return "Dmnk.Blazor.Mvvm";
    }

    private static bool InheritsFromComponentBase(INamedTypeSymbol typeSymbol, Compilation compilation)
    {
        // For Razor components that haven't been generated yet, we can't check inheritance
        // So we'll skip the check if the type has no base type (incomplete symbol)
        if (typeSymbol.BaseType == null || typeSymbol.BaseType.SpecialType == SpecialType.System_Object)
            return true; // Assume it's valid, let runtime catch the error

        var currentType = typeSymbol.BaseType;
        while (currentType != null)
        {
            if (currentType.Name == "ComponentBase" && 
                currentType.ContainingNamespace?.ToDisplayString() == "Microsoft.AspNetCore.Components")
            {
                return true;
            }
            currentType = currentType.BaseType;
        }

        return false;
    }

    private record struct ViewModelInfo(string ViewModelType, string ViewType);
    
    private record struct ViewModelInfoWithDiagnostics(
        string ViewModelType, 
        string ViewType, 
        DiagnosticInfo[] Diagnostics);
    
    private record struct DiagnosticInfo(
        DiagnosticDescriptor Descriptor, 
        Location Location, 
        params object[] MessageArgs);
}

