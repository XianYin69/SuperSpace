using Microsoft.CommandPalette.Extensions.Toolkit;
using Microsoft.CommandPalette.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSpace.Addition.i18n;
using static SuperSpace.Addition.i18n.i18n;
using SuperSpace.Addition.PageSupport;
using static SuperSpace.Addition.PageSupport.RecentFile;

namespace SuperSpace.Pages.OfficeRootPage;

internal sealed partial class OfficeRootPage : ListPage
{
    public OfficeRootPage()
    {
        Title = T("");
        Name = T("");
    }
    public override IListItem[] GetItems()
    {
        var items = new List<IListItem>
        {
            new ListItem(new WordPage())
            {
                Title = T(""),
                Subtitle = T("")
            },
            new ListItem(new PowerPointPage())
            {
                Title = T(""),
                Subtitle = T("")
            },
            new ListItem(new ExcelPage())
            {
                Title = T(""),
                Subtitle = T("")
            }
        };
        RecentFile();
    }
}


