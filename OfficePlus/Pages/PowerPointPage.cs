//It is a new page of office-plus
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using System.Collections.Generic;

namespace OfficePlus.Pages;

internal sealed partial class PowerPointPage : ListPage
{
    public PowerPointPage()
    {
        Icon = IconHelpers.FromRelativePath("Assets\\OfficePlusPowerPointPageIcon.png");
        Title = "PowerPoint";
        Name = "";
    }
    public override IListItem[] GetItems()
    {
        var items = new List<IListItem>
        {
            new ListItem(new NoOpCommand())
            {
                Title = "新建",
            },
            new ListItem(new NoOpCommand())
            {
                Title = "打开",
            }
        };
        return items.ToArray();
    }
}
