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
            new ListItem(new RunExcelCommand("EXCEL.EXE"))
            {
                Title = "新建工作表",
                Subtitle = "启动Excle并新建工作表",
                Icon = IconHelpers.FromRelativePath("Assets\\FluentColorDocumentAdd48.png"),
            },
            new ListItem(new ExcelRecentPage())
            {
                Title = "打开工作表",
                Subtitle = "打开最近使用的Excel工作表",
                Icon = IconHelpers.FromRelativePath("Assets\\FluentColorDocumentEdit24.png"),
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
{
    private const int MaxRecentItems = 20;

    //Define the suffix filter related to Excel
    private readonly HashSet<string> _excelExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        "xls", "xlsx", "xlsm", "xlsb", "xltx", "xltm", "csv", "xml", "xlt", "xlm", "slk", "dlf"
    };
    public ExcelRecentPage()
    {
        Title = "最近的 Excel 文档";
        Icon = IconHelpers.FromRelativePath("Assets\\FluentColorDocumentEdit24.png");
    }
    public override IListItem[] GetItems()
    {
        var items = new List<IListItem>();
        string recentDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "Microsoft", "Office", "Recent");
        try
        {
            if (Directory.Exists(recentDir))
            {
                // 1. Get all files
                var files = new DirectoryInfo(recentDir).GetFiles("*")
                    .OrderByDescending(f => f.LastWriteTime);

                // 2. Apply filter
                var filteredFiles = files
                    .Where(f => IsExcelFile(f.Name))
                    .Take(MaxRecentItems);
                foreach (var file in filteredFiles)
                {
                    // remove suffix(.lnk)
                    string DisplayName = file.Name.EndsWith(".lnk", StringComparison.OrdinalIgnoreCase)
                        ? Path.GetFileNameWithoutExtension(file.Name)
                        : file.Name;
                    items.Add(new ListItem(new RunExcelCommand(file.FullName))
                    {
                        Title = DisplayName,
                        Subtitle = $"上次修改: {file.LastWriteTime.ToShortDateString()}",
                        Icon = IconHelpers.FromRelativePath("Assets\\FluentColorDocument48.png")
                    });
                }
            }
            if (items.Count == 0)
            {
                items.Add(new ListItem(new NoOpCommand()) { Title = "未发现最近的 Excel 文档 " });
            }
        }
        catch (Exception ex)
        {
            items.Add(new ListItem(new NoOpCommand()) { Title = $"读取失败： {ex.Message}" });
        }
        return items.ToArray();
    }
    // IsExcelFile method
    private bool IsExcelFile(string fileName)
    {
        /*
         * check suffix
         */
        return _excelExtensions.Any(ext => fileName.Contains(ext, StringComparison.OrdinalIgnoreCase));
    }
}