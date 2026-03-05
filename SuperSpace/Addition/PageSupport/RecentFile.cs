using Microsoft.CommandPalette.Extensions.Toolkit;
using Microsoft.CommandPalette.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSpace.Addition.i18n;
using static SuperSpace.Addition.i18n.i18n;
using Windows.ApplicationModel.Appointments;


namespace SuperSpace.Addition.PageSupport
{
    internal class RecentFile
    {
        const int MaxRecentItems = 20;
        public List<IListItem> items { get; } = new();
        public RecentFile(string PathSingle_1, string PathSingle_2, string PathSingle_3, bool UseFilter, List<string> SuffixName)
        {
            try
            {
                string recentDir = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    PathSingle_1,
                    PathSingle_2,
                    PathSingle_3);

                if (Directory.Exists(recentDir))
                {
                    var recentFiles = Directory
                        .GetFiles(recentDir)
                        .OrderByDescending(f => File.GetLastWriteTimeUtc(f))
                        .Take(MaxRecentItems);

                    if (UseFilter)
                    {
                        FileFilter(recentDir, SuffixName);
                    }
                    else
                    {
                        foreach (var file in recentFiles)
                        {
                            // 显示为文件名（包含扩展名），选择时导航到一个会打开该文件的页面
                            string displayName = Path.GetFileNameWithoutExtension(file);
                            items.Add(new ListItem(new OpenFileCommand(file))
                            {
                                Title = displayName,
                                Icon = IconHelpers.FromRelativePath("Assets\\FluentColorDocument48.png")
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // 如果枚举失败，显示错误提示项
                items.Add(new ListItem(new NoOpCommand()) { Title = T("SuperSpacePage.CantFindFile", ex.Message) });
            }
        }

        //Select current file
        public void FileFilter(string Path , List<string> SuffixName)
        {
            var filterList = SuffixName.Select(s => s.StartsWith('.') ? s.ToLower() : "." + s.ToLower()).ToList();
            var files = Directory.GetFiles(Path)
                .Where(f => filterList.Contains(System.IO.Path.GetExtension(f).ToLower()))
                .OrderByDescending(f => File.GetLastWriteTimeUtc(f))
                .Take(MaxRecentItems);
            foreach (var file in files)
            {
                items.Add(new ListItem(new OpenFileCommand(file))
                {
                    Title = System.IO.Path.GetFileNameWithoutExtension(file),
                    Subtitle = T(filterList.FirstOrDefault() + "_sub")
                });
            }    
        }
    }
}