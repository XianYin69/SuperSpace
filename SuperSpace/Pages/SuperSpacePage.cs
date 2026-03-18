// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using static SuperSpace.Addition.i18n.i18n;

namespace SuperSpace.Pages;

//Start Page
internal sealed partial class SuperSpacePage : ListPage
{
    public SuperSpacePage()
    {
        Icon = IconHelpers.FromRelativePath("Assets\\SuperSpacePageIcon.png");
        Title = T("");
        Name = T("");
    }

    public override IListItem[] GetItems()
    {
        var items = new List<IListItem>
        {
            new ListItem(new NoOpCommand())
            {
                Title = T("Office.Title"),
                Subtitle = T("Office.Subtitle"),
                Icon = IconHelpers.FromRelativePath("Assets\\FluentColorAdd24.png")
            },
            new ListItem(new NoOpCommand())
            {
                Title = T("Developing.Title"),
                Subtitle = T("Developing.Subtitle"),
                Icon = IconHelpers.FromRelativePath("Assets\\FluentColorAdd24.png")
            },
            new ListItem(new NoOpCommand())
            {
                Title = T("Gaming.Title"),
                Subtitle = T("Gaming.Subtitle"),
                Icon = IconHelpers.FromRelativePath("Assets\\FluentColorAdd24.png")
            }
        };
        return items.ToArray();
    }
}