// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using OfficePlus.Pages;

namespace OfficePlus;

internal sealed partial class OfficePlusPage : ListPage
{
    public OfficePlusPage()
    {
        Icon = IconHelpers.FromRelativePath("Assets\\OfficePlusPageIcon.png");
        Title = "OfficePlus";
        Name = "Open or creat your file";
    }

    public override IListItem[] GetItems()
    {
        return [
            new ListItem(new NoOpCommand()) { Title = "注意：这是一个非微软官方拓展" },
            new ListItem(new WordPage()) { Title = "Word" },
            new ListItem(new ExcelPage ()) { Title = "Excel" },
            new ListItem(new PowerPointPage()) { Title = "PowerPoint" }
        ];
    }
}
