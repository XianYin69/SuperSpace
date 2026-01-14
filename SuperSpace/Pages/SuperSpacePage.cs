// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using System.Text.Json;
using System.Globalization;
using SuperSpace.Pages.i18n;
using static SuperSpace.Pages.i18n.i18n;

namespace SuperSpace.Pages;

internal sealed partial class SuperSpacePage : ListPage
{
    private const int MaxRecentItems = 20;

    public SuperSpacePage()
    {
        Icon = IconHelpers.FromRelativePath("Assets\\SuperSpacePageIcon.png");
        Title = "SuperSpace";
        Name = "Open or create your file";
    }

    public override IListItem[] GetItems()
    {
        var items = new List<IListItem>
        {
            new ListItem(new NoOpCommand()) { Title = T("SuperSpacePage.Item1") },
            new ListItem(new WordPage()) { Title = T("SuperSpacePage.Item2") },
            new ListItem(new ExcelPage()) { Title = T("SuperSpacePage.Item3") },
            new ListItem(new PowerPointPage()) { Title = T("SuperSpacePage.Item4") }
        };

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

        return items.ToArray();
    }
}