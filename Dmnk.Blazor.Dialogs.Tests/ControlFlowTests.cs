using Dmnk.Blazor.Dialogs.Tests.BasicCustomDialog;

namespace Dmnk.Blazor.Dialogs.Tests;

public class ControlFlowTests
{
    [Test, Timeout(500)]
    public async Task Custom_Dialog()
    {
        var controller = new HeadlessVmDialogController();
        controller.Register<BasicCustomDialogViewModel>();
        
        var vmHost = new HostViewModel(controller);

        var showDialogCommandTask = vmHost.ShowDialogCommand.ExecuteAsync(null);
        
        var vmDlg = controller.GetLastOpenedOfType<BasicCustomDialogViewModel>();
        Assert.That(vmDlg.SetOnDismissed, Is.False);
        Assert.That(vmDlg.SetOnDismissedAsync, Is.False);
        
        vmDlg.InputText = "50";
        vmDlg.InputInt = 50;
        vmDlg.ConfirmCommand.Execute(null);

        await showDialogCommandTask;
        
        Assert.That(vmHost.IntDlgResult, Is.EqualTo(50));
        Assert.That(vmHost.StrDlgResult, Is.EqualTo("50"));
        Assert.That(vmDlg.SetOnDismissed, Is.True);
        Assert.That(vmDlg.SetOnDismissedAsync, Is.True);
    }
}