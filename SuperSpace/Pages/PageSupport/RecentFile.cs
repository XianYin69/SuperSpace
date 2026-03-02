using Microsoft.CommandPalette.Extensions.Toolkit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSpace.Pages.PageSupport
{
    internal class RecentFile
    {
        try
        {
            string recentDir = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "Microsoft",
                "Office",
                "Recent");

            if (Directory.Exists(recentDir))
            {
                var recentFiles = Directory
                    .GetFiles(recentDir)
                    .OrderByDescending(f => File.GetLastWriteTimeUtc(f))
                    .Take(MaxRecentItems);

                foreach (var file in recentFiles)
                {
                    // 显示为文件名（包含扩展名），选择时导航到一个会打开该文件的页面
                    string displayName = Path.GetFileNameWithoutExtension(file);
        items.Add(new ListItem(new OpenFileCommand(file)) {
                        Title = displayName,
                        Icon = IconHelpers.FromRelativePath("Assets\\FluentColorDocument48.png")
                    });
                }
            }
        }
        catch (Exception ex)
        {
            // 如果枚举失败，显示错误提示项
            items.Add(new ListItem(new NoOpCommand()) { Title = T("SuperSpacePage.CantFindFile", ex.Message) });
        }
    }
}
