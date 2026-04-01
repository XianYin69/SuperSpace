using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using SuperSpace.Pages.Game.Steam;
using static SuperSpace.Addition.i18n.i18n;

namespace SuperSpace.Pages.Game;

internal sealed partial class rootGamePage : ListPage
{
    public rootGamePage()
    {
        Icon = IconHelpers.FromRelativePath("Assets:\\");
        Title = T("");
    }

    public override IListItem[] GetItems()
    {
        var items = new List<IListItem>
        {
            new ListItem(new SteamPage())
            {
                Title = T(""),
                Subtitle = T(""),
                Icon = IconHelpers.FromRelativePath("Assets:\\")
            }
        };
        return items.ToArray();
    }
}
