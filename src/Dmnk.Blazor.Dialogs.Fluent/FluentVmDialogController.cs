using Dmnk.Blazor.Dialogs.Api;
using Dmnk.Blazor.Dialogs.DefaultDialogs;
using Dmnk.Blazor.Dialogs.Fluent.DefaultDialogs;
using Dmnk.Icons.Blazor.Fluent;
using Microsoft.FluentUI.AspNetCore.Components;
using Icon = Dmnk.Icons.Core.Icon;
using MsIcons = Microsoft.FluentUI.AspNetCore.Components.Icons.Filled.Size24;

namespace Dmnk.Blazor.Dialogs.Fluent;

/// <summary>
/// Implements <see cref="BlazorVmDialogController"/> by registering FluentUI versions of the
/// default dialogs.
/// <p>IVmDialogController: <inheritdoc cref="IVmDialogController"/>.</p>
/// </summary>
public class FluentVmDialogController : BlazorVmDialogController
{
    /// <summary> See <see cref="FluentVmDialogController"/> </summary>
    public FluentVmDialogController()
    {
        Register(typeof(InputDialogView<>), typeof(InputDialogViewModel<>));
        Register<MessageBoxView, MessageBoxViewModel>();
        Register<ConfirmationDialogView, ConfirmationDialogViewModel>();
    }

    /// <summary> <inheritdoc/> </summary>
    public override Icon DefaultIconForIntent(MessageBoxType type) =>
        type switch
        {
            MessageBoxType.Success => new MsIcons.CheckmarkCircle().WithColor(Color.Success).ToGenericIcon(),
            MessageBoxType.Error => new MsIcons.ErrorCircle().WithColor(Color.Error).ToGenericIcon(),
            MessageBoxType.Warning => new MsIcons.Warning().WithColor(Color.Warning).ToGenericIcon(),
            MessageBoxType.Confirmation => new MsIcons.Question().WithColor(Color.Neutral).ToGenericIcon(),
            _ => new MsIcons.Info().WithColor(Color.Info).ToGenericIcon(),
        };
}