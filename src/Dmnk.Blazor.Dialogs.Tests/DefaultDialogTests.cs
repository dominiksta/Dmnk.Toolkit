using Microsoft.AspNetCore.Components;
using Dmnk.Blazor.Dialogs.DefaultDialogs;

namespace Dmnk.Blazor.Dialogs.Tests;

public class DefaultDialogTests
{
    [Test]
    [TestCase("Msg", null, false)]
    [TestCase(null, "<div>Msg</div>", false)]
    [TestCase(null, null, true)]
    public async Task MessageBox(string? msg, string? msgMarkup, bool fail)
    {
        var controller = new HeadlessVmDialogController();
        
        await controller.Show(
            new VmDialogParameters() { Title = "" },
            new MessageBoxViewModel()
            {
                Type = MessageBoxType.Confirmation,
                Message = msg,
                MessageMarkup = msgMarkup != null ? (MarkupString) msgMarkup : null,
                ActionConfirmText = "Oki"
            }
        );

        var vm = controller.GetLastOpenedOfType<MessageBoxViewModel>();
        if (fail) Assert.Throws<InvalidOperationException>(() => vm.DisplayMessage());
        else Assert.DoesNotThrow(() => vm.DisplayMessage());
    }
    
    [Test]
    public async Task InputDialog()
    {
        var controller = new HeadlessVmDialogController();
        
        await controller.Show(
            new VmDialogParameters() { Title = "" },
            new InputDialogViewModel<string>()
            {
                Value = "default-value",
                Validate = v => v == "default-value" ? "error" : null
            }
        );

        var vm = controller.GetLastOpenedOfType<InputDialogViewModel<string>>();
        
        Assert.That(vm.Value, Is.EqualTo("default-value"));
        Assert.That(vm.ValidationError, Is.EqualTo("error"));
        
        await vm.CloseCommand.ExecuteAsync(null);
        Assert.That(vm.Dialog.Closed, Is.False);

        vm.Value = "new-value";
        
        Assert.That(vm.ValidationError, Is.Null);
        
        vm.CloseCommand.Execute(null);
        Assert.That(vm.Dialog.Closed, Is.True);
    }
}