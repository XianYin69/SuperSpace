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

namespace OfficePlus.Pages;

internal sealed partial class OfficePlusPage : ListPage
{
    private const int MaxRecentItems = 20;

    public OfficePlusPage()
    {
        Icon = IconHelpers.FromRelativePath("Assets\\OfficePlusPageIcon.png");
        Title = "OfficePlus";
        Name = "Open or create your file";
    }

    public override IListItem[] GetItems()
    {
        var items = new List<IListItem>
        {
            new ListItem(new NoOpCommand()) { Title = "!!!注意：本扩展为非微软官方扩展!!!" },
            new ListItem(new WordPage()) { Title = "Word" },
            new ListItem(new ExcelPage()) { Title = "Excel" },
            new ListItem(new PowerPointPage()) { Title = "PowerPoint" }
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
            items.Add(new ListItem(new NoOpCommand()) { Title = $"无法读取最近文件：{ex.Message}" });
        }

        return items.ToArray();
    }
}

//i18n support
public static class i18n
{
    private static JsonElement _currentLanguageData;
    private static readonly string DefaultLang = "en-US";
    static i18n()
    {
        try
        {
            string jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Language.json");
            string jsonContent = File.ReadAllText(jsonPath);
            using var doc  = JsonDocument.Parse(jsonContent);
            var root = doc.RootElement;
            // get system language
            string uiCulture = CultureInfo.CurrentUICulture.Name;
            //Matching Logic Priority
            //Exact Match
            if (root.TryGetProperty(uiCulture, out var exacMatch))
            {
                _currentLanguageData = exacMatch.Clone();
            }
            //Language Group Match
            else if (uiCulture.StartsWith("zh") && root.TryGetProperty("zh-TW", out var zhFallBack))
            {
                _currentLanguageData = zhFallBack.Clone();
            }
            //Base Languagge Match
            else if (root.TryGetProperty(uiCulture.Split("-")[0], out var baseLanguage))
            {
                _currentLanguageData = baseLanguage.Clone();
            }
            //Back to English
            else
            {
                _currentLanguageData = root.GetProperty(DefaultLang).Clone();
            }
        }
        catch
        {
            //code fix languge
            string fallbackjson = @"{
               ""en-US"": {
                    ""OfficePlusPage"": {
                    ""Item2"": ""Word"",
                    ""Item3"": ""Excel"",
                    ""Item4"": ""PowerPoint"",
                    ""Item1"": ""WARNING: it is an unofficial plugin for Microsoft Office""
                    },
                    ""WordPage"": {
                        ""CreateNewDoc"": ""Create New Document"",
                        ""CreateNewDocSub"": ""Open Microsoft Word and create a new document"",
                        ""OpenDoc"": ""Open Document"",
                        ""OpenDocSub"": ""Open recent document"",
                        ""WordRecentPage"": ""Recent Documents""
                    },
                    ""ExcelPage"": {
                        ""CreateNewDoc"": ""Create New Workbook"",
                        ""CreateNewDocSub"": ""Open Microsoft Excel and create a new workbook"",
                        ""OpenDoc"": ""Open Workbook"",
                        ""OpenDocSub"": ""Open recent workbook"",
                        ""WordRecentPage"": ""Recent Workbooks""
                    },
                    ""PowerPointPage"": {
                        ""CreateNewDoc"": ""Create New Presentation"",
                        ""CreateNewDocSub"": ""Open Microsoft PowerPoint and create a new presentation"",
                        ""OpenDoc"": ""Open Presentation"",
                        ""OpenDocSub"": ""Open recent presentation"",
                        ""WordRecentPage"": ""Recent Presentations""
                    }
                }     
            }";
            var fallbackDoc = JsonDocument.Parse(fallbackjson);
            _currentLanguageData = fallbackDoc.RootElement.GetProperty(DefaultLang).Clone();
        }
    }
    public static string T(string path)
    {
        try
        {
            if (string.IsNullOrEmpty(path)) return path;
            string[] keys = path.Split('.');
            JsonElement current = _currentLanguageData;
            foreach (string key in keys)
            {
                if (current.ValueKind == JsonValueKind.Object && current.TryGetProperty(key, out JsonElement next))
                {
                    current = next;
                }
                else
                {
                    return path;
                }
            }
            return current.ValueKind == JsonValueKind.String ? current.GetString() : path;
        }
        catch
        {
            return path;
        }
    }
}