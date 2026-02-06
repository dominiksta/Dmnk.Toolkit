using Dmnk.Blazor.Dialogs.DefaultDialogs;
using Dmnk.Blazor.Dialogs.Fluent.DefaultDialogs;
using Dmnk.Icons.Blazor.Fluent;
using Microsoft.FluentUI.AspNetCore.Components;
using Icon = Dmnk.Icons.Core.Icon;
using MsIcons = Microsoft.FluentUI.AspNetCore.Components.Icons.Filled.Size24;

namespace Dmnk.Blazor.Dialogs.Fluent;

public class FluentVmDialogController : BlazorVmDialogController
{
    public FluentVmDialogController()
    {
        Register(typeof(InputDialogView<>), typeof(InputDialogViewModel<>));
        Register<MessageBoxView, MessageBoxViewModel>();
        Register<ConfirmationDialogView, ConfirmationDialogViewModel>();
    }

    public override Icon DefaultIconForIntent(MessageBoxType type) =>
        type switch
        {
            MessageBoxType.Success => new MsIcons.CheckmarkCircle().WithColor(Color.Success).ToIcon(),
            MessageBoxType.Error => new MsIcons.ErrorCircle().WithColor(Color.Error).ToIcon(),
            MessageBoxType.Warning => new MsIcons.Warning().WithColor(Color.Warning).ToIcon(),
            MessageBoxType.Confirmation => new MsIcons.Question().WithColor(Color.Neutral).ToIcon(),
            _ => new MsIcons.Info().WithColor(Color.Info).ToIcon(),
        };
}