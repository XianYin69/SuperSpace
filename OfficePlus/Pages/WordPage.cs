//It is a new page of office-plus
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using System.Collections.Generic;

namespace OfficePlus.Pages;
internal sealed partial class WordPage : ListPage
{
    public WordPage()
    {
        Icon = IconHelpers.FromRelativePath("Assets\\OfficePlusWordPageIcon.png");
        Title = "Word";
        Name = "";
    }
    public override IListItem[] GetItems()
    {
        var items = new List<IListItem>
        {
            new ListItem(new NoOpCommand())
            {
                Title = "新建",
                Icon = "\uF8AA"
            },
            new ListItem(new NoOpCommand())
            {
                Title = "打开",
            }
        };
        return items.ToArray();
    }
}