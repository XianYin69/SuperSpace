//It is a new page of office-plus
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using System.Collections.Generic;
using static SuperSpace.Addition.i18n.i18n;
using SuperSpace.Addition.PageSupport;
using SuperSpace.Addition.OpenApp;

namespace SuperSpace.Pages.Offce.MicrosoftOffice.PowerPoint;

internal sealed partial class PowerPointPage : ListPage
{
    List<string> suffix_name = new List<string>() { ".ppt", ".pptx" };
    public PowerPointPage()
    {
        Icon = IconHelpers.FromRelativePath("Assets\\SuperSpacePowerPointPageIcon.png");
        Title = T("Office.Microsoft.PowerPointPage.Title");
    }
    public override IListItem[] GetItems()
    {
        var items = new List<IListItem>
        {
            new ListItem(new OpenApp("POWERPNT.EXE"))
            {
                Title = T("Office.Microsoft.PowerPointPage.CreateNewFile"),
                Subtitle = T("Office.Microsoft.PowerPointPage.Subtitle"),
                Icon = IconHelpers.FromRelativePath("Assets\\FluentColorDocumentAdd48.png")
            }
        };
        items.AddRange(new RecentFile("Microsoft", "Office", "Recent", true, suffix_name)
            .items);
        return items.ToArray();
    }
}