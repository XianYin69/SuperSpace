//It is a new page of office-plus
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace OfficePlus.Pages;

internal sealed partial class WordPage : ListPage
{
    public WordPage()
    {
        Icon = IconHelpers.FromRelativePath("Assets\\OfficePlusWordPageIcon.png");
        Title = "Word";
        Name = "";
    }
    public override IListItem[] GetItems()
    {
        var items = new List<IListItem>
        {
            new ListItem(new RunWordCommand("WINWORD.EXE"))
            {
                Title = "新建文档",
                Subtitle = "启动Word并新建文档",
                Icon = IconHelpers.FromRelativePath("Assets\\FluentColorDocumentAdd48.png"),
            },
            new ListItem(new WordRecentPage())
            {
                Title = "打开文档",
                Subtitle = "打开最近使用的Word文档",
                Icon = IconHelpers.FromRelativePath("Assets\\FluentColorDocumentEdit24.png"),
            }
        };
        return items.ToArray();
    }
}
internal sealed class RunWordCommand : InvokableCommand
{
    private readonly string _executable;
    public RunWordCommand(string executable)
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

internal sealed partial class WordRecentPage : ListPage
{
    private const int MaxRecentItems = 20;

    //Define the suffix filter related to Word
    private readonly HashSet<string> _WordExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        "xls", "xlsx", "xlsm", "xlsb", "xltx", "xltm", "csv", "xml", "xlt", "xlm", "slk", "dlf"
    };
    public WordRecentPage()
    {
        Title = "最近的 Word 文档";
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
                    .Where(f => IsWordFile(f.Name))
                    .Take(MaxRecentItems);
                foreach (var file in filteredFiles)
                {
                    // remove suffix(.lnk)
                    string DisplayName = file.Name.EndsWith(".lnk", StringComparison.OrdinalIgnoreCase)
                        ? Path.GetFileNameWithoutExtension(file.Name)
                        : file.Name;
                    items.Add(new ListItem(new RunWordCommand(file.FullName))
                    {
                        Title = DisplayName,
                        Subtitle = $"上次修改: {file.LastWriteTime.ToShortDateString()}",
                        Icon = IconHelpers.FromRelativePath("Assets\\FluentColorDocument48.png")
                    });
                }
            }
            if (items.Count == 0)
            {
                items.Add(new ListItem(new NoOpCommand()) { Title = "未发现最近的 Word 文档 " });
            }
        }
        catch (Exception ex)
        {
            items.Add(new ListItem(new NoOpCommand()) { Title = $"读取失败： {ex.Message}" });
        }
        return items.ToArray();
    }
    // IsWordFile method
    private bool IsWordFile(string fileName)
    {
        /*
         * check suffix
         */
        return _WordExtensions.Any(ext => fileName.Contains(ext, StringComparison.OrdinalIgnoreCase));
    }
}