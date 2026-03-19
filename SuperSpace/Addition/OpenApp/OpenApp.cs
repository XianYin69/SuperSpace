using Microsoft.CommandPalette.Extensions.Toolkit;
using System.Diagnostics;

namespace SuperSpace.Addition.OpenApp;

//Open application
internal sealed partial class OpenApp : InvokableCommand
{
    private readonly string _excutable;

    public OpenApp(string excutable)
    {
        _excutable = excutable;
    }

    public override CommandResult Invoke()
    {
        try
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = _excutable,
                UseShellExecute =true
            });
            return CommandResult.Dismiss();
        } catch
        {
            return CommandResult.KeepOpen();
        }
    }
}