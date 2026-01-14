//It is a new page of office-plus
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using SuperSpace.Pages.i18n;
using static SuperSpace.Pages.i18n.i18n;

namespace SuperSpace.Pages;

internal sealed partial class PowerPointPage : ListPage
{
    public PowerPointPage()
    {
        Icon = IconHelpers.FromRelativePath("Assets\\SuperSpacePowerPointPageIcon.png");
        Title = "PowerPoint";
        Name = "";
    }
    public override IListItem[] GetItems()
    {
        var items = new List<IListItem>
        {
            new ListItem(new RunPowerPointCommand("POWERPNT.EXE"))
            {
                Title = T("PowerPointPage.CreateNewDoc"),
                Subtitle = T("PowerPointPage.CreateNewDocSub"),
                Icon = IconHelpers.FromRelativePath("Assets\\FluentColorDocumentAdd48.png")
            },
            new ListItem(new PowerPointRecentPage())
            {
                Title = T("PowerPointPage.OpenDoc"),
                Subtitle = T("PowerPointPage.OpenDocSub"),
                Icon = IconHelpers.FromRelativePath("Assets\\FluentColorDocumentEdit24.png")
            }
        };
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

internal sealed partial class PowerPointRecentPage : ListPage
{
    private const int MaxRecentItems = 10;

    // 定义 PowerPoint 相关的后缀过滤器
    private readonly HashSet<string> _pptExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".pptx", ".ppt", ".potx", ".ppsx", "pot", ".pps", "potm", "ppsm", "pptm"
    };

    public PowerPointRecentPage()
    {
        Title = T("PowerPointPage.WordRecentPage");
        Icon = IconHelpers.FromRelativePath("Assets\\FluentColorDocumentEdit24.png");
    }

    public override IListItem[] GetItems()
    {
        var items = new List<IListItem>();
        string recentDir = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "Microsoft", "Office", "Recent");

        try
        {
            if (Directory.Exists(recentDir))
            {
                // 1. 获取所有文件
                var files = new DirectoryInfo(recentDir).GetFiles("*")
                    .OrderByDescending(f => f.LastWriteTimeUtc);

                // 2. 应用过滤器
                var filteredFiles = files
                    .Where(f => IsPowerPointFile(f.Name))
                    .Take(MaxRecentItems);

                foreach (var file in filteredFiles)
                {
                    // 移除 .lnk 后缀显示更美观
                    string displayName = file.Name.EndsWith(".lnk", StringComparison.OrdinalIgnoreCase)
                        ? Path.GetFileNameWithoutExtension(file.Name)
                        : file.Name;

                    items.Add(new ListItem(new RunPowerPointCommand(file.FullName))
                    {
                        Title = displayName,
                        Subtitle = T("PowerPointPage.LaterEdit", file.LastWriteTime.ToShortDateString()),
                        Icon = IconHelpers.FromRelativePath("Assets\\FluentColorDocument48.png")
                    });
                }
            }

            if (items.Count == 0)
            {
                items.Add(new ListItem(new NoOpCommand()) { Title = T("PowerPointPage.CantFindFile") });
            }
        }
        catch (Exception ex)
        {
            items.Add(new ListItem(new NoOpCommand()) { Title = T("PowerPointPage.ReadError", ex.Message) });
        }

        return items.ToArray();
    }

    // 过滤器方法：检查文件名是否包含 PPT 关键字或后缀
    private bool IsPowerPointFile(string fileName)
    {
        // 逻辑：由于 Office Recent 下通常是 "文件名.pptx.lnk"
        // 我们检查文件名是否包含 PPT 相关的后缀
        return _pptExtensions.Any(ext => fileName.Contains(ext, StringComparison.OrdinalIgnoreCase));
    }
}