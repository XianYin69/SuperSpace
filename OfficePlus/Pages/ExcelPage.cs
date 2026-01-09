//It is a new page of office-plus
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace OfficePlus.Pages;

internal sealed partial class ExcelPage : ListPage
{
    public ExcelPage()
    {
        Icon = IconHelpers.FromRelativePath("Assets\\OfficePlusExcelPageIcon.png");
        Title = "Excel";
        Name = "";
    }
    public override IListItem[] GetItems()
    {
        var items = new List<IListItem>
        {
            new ListItem(new NoOpCommand())
            {
                Title = "新建",
                Icon = IconHelpers.FromRelativePath("Asset\\FluentColorDocumentAdd48.png")
            },
            new ListItem(new NoOpCommand())
            {
                Title = "打开",
                Icon = IconHelpers.FromRelativePath("Asset\\FluentColorDocumentEdit24.png")
            }
        };
        return items.ToArray();
    }
}
internal sealed class RunExcelCommand :　InvokableCommand
{
    private readonly string _executable;
    public RunExcelCommand(string executable)
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

internal sealed partial class ExcelRecentPage :　ListPage
