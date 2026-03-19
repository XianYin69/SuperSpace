//It is a new page of office-plus
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using System.Collections.Generic;
using System.Diagnostics;
using static SuperSpace.Addition.i18n.i18n;
using SuperSpace.Addition.PageSupport;

namespace SuperSpace.Pages.Offce.MicrosoftOffice.PowerPoint;

internal sealed partial class PowerPointPage : ListPage
{
    List<string> suffix_name = new List<string>() { ".ppt", ".pptx" };
    public PowerPointPage()
    {
        Icon = IconHelpers.FromRelativePath("Assets\\SuperSpacePowerPointPageIcon.png");
        Title = T("Office.Microsoft.PowerPointPage.Title");
    }
    public override IListItem[] GetItems()
    {
        var items = new List<IListItem>
        {
            new ListItem(new RunPowerPointCommand("POWERPNT.EXE"))
            {
                Title = T("Office.Microsoft.PowerPointPage.CreateNewFile"),
                Subtitle = T("Office.Microsoft.PowerPointPage.Subtitle"),
                Icon = IconHelpers.FromRelativePath("Assets\\FluentColorDocumentAdd48.png")
            }
        };
        items.AddRange(new RecentFile("Microsoft", "Office", "Recent", true, suffix_name)
            .items);
        return items.ToArray();
    }
}
// Add a starting process class
internal sealed partial class RunPowerPointCommand :　InvokableCommand
{
    private readonly string _executable;
    
    public RunPowerPointCommand(string executable)
    {
        _executable = executable;
    }

    public override CommandResult Invoke()
    {
        try
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = _executable,
                UseShellExecute = true
            });
            return CommandResult.Dismiss();
        }
        catch
        {
            return CommandResult.KeepOpen();
        }
    }
}