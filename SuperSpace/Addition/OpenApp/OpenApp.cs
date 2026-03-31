using Microsoft.CommandPalette.Extensions.Toolkit;
using System.Diagnostics;

namespace SuperSpace.Addition.OpenApp;

//Open application
internal sealed partial class OpenApp : InvokableCommand
{
    private readonly string _excutable;
    private readonly bool _useShellExecute;

    public OpenApp(string excutable, bool useShellExecute)
    {
        _excutable = excutable;
        _useShellExecute = useShellExecute;
    }

    public override CommandResult Invoke()
    {
        try
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = _excutable,
                UseShellExecute = _useShellExecute
            });
            return CommandResult.Dismiss();
        } catch
        {
            return CommandResult.KeepOpen();
        }
    }
}