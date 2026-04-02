using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using static SuperSpace.Addition.i18n.i18n;


namespace SuperSpace.Pages.Dev;

internal sealed partial class rootDevPage : ListPage
{
    public rootDevPage()
    {
        Icon = IconHelpers.FromRelativePath("Assets\\");
        Title = T("");
    }

    public override IListItem[] GetItems()
    {
        var items = new List<IListItem> { 
            new ListItem(new NoOpCommand())
            {
                Icon = IconHelpers.FromRelativePath("Assets\\"),
                Title = T(""),
                Subtitle = T("")
            }
        };
        return items.ToArray();
    }
}
