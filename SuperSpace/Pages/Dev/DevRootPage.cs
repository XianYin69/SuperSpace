using Microsoft.CommandPalette.Extensions.Toolkit;
using Microsoft.CommandPalette.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSpace.Addition.i18n;
using static SuperSpace.Addition.i18n.i18n;

namespace SuperSpace.Pages.Dev;

internal sealed partial class GameRootPage : ListPage
{
    public GameRootPage()
    {
        Title = T("Developing.Title");
    }

    public override IListItem[] GetItems()
    {
        var items = new List<IListItem>
        {
            new ListItem(new NoOpCommand)
            {
                Title = T()
            }
        }
    }
}

