// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using OfficePlus.Pages;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace OfficePlus;

internal sealed partial class OfficePlusPage : ListPage
{
    public OfficePlusPage()
    {
        Icon = IconHelpers.FromRelativePath("Assets\\OfficePlusPageIcon.png");
        Title = "OfficePlus";
        Name = "Open or creat your file";
    }

    //getOfficeRecentFiles
    private List<Results> getOfficeRecentFiles(string search)
    {
        var results = new List<Result>();
        //Local File
        string officeRecentPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            @"Microsoft\Office\Recent"
        );
        if (Directory.Exists(officeRecentPath))
        {
            DirectoryInfo dir = new DirectoryInfo(officeRecentPath);
            var file = dir
                .GetFiles("*.lnk")
                .OrderByDescending(f => f.LastWriteTime)
                .Take(20);
        }

        foreach (var file in files)
        {
            //filter links
            if (string.IsNullOrEmpty(search) || file.Name.ToLower().Contains(search.ToLower()))
            {
                results.Add(file);
            }
        }
        {
            
        }
    }
    public override IListItem[] GetItems()
    {
        return [
            new ListItem(new NoOpCommand()) { Title = "!!!注意：本扩展为非微软官方扩展!!!" },
            new ListItem(new WordPage()) { Title = "Word" },
            new ListItem(new ExcelPage ()) { Title = "Excel" },
            new ListItem(new PowerPointPage()) { Title = "PowerPoint" }
        ];
    }
}
