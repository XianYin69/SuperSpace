//It is a new page of office-plus
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using System.Collections.Generic;
using System.Diagnostics;
using static SuperSpace.Addition.i18n.i18n;
using SuperSpace.Addition.PageSupport;

namespace SuperSpace.Pages.Offce.MicrosoftOffice.Excel;

//ExcelPage for open or creat a table file
internal sealed partial class ExcelPage : ListPage
{
    List<string> suffix_name = new List<string>() { ".xls", ".xlsx" };
    public ExcelPage()
    {
        Icon = IconHelpers.FromRelativePath("Assets\\SuperSpaceExcelPageIcon.png");
        Title = "Excel";
        Name = "";
    }
    public override IListItem[] GetItems()
    {
        var items = new List<IListItem>
        {
            new ListItem(new RunExcelCommand("EXCEL.EXE"))
            {
                Title = T("ExcelPage.CreateNewDoc"),
                Subtitle = T("ExcelPage.CreateNewDocSub"),
                Icon = IconHelpers.FromRelativePath("Assets\\FluentColorDocumentAdd48.png"),
            }
        };
        items.AddRange(new RecentFile("Microsoft", "Office", "Recent", true, suffix_name)
            .items);
        return items.ToArray();
    }
}

//It is a function to show recent file and I need to new a package to react that.
internal sealed partial class RunExcelCommand :　InvokableCommand
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
