//It is a new page of office-plus
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using System.Collections.Generic;
using static SuperSpace.Addition.i18n.i18n;
using SuperSpace.Addition.PageSupport;
using SuperSpace.Addition.OpenApp;

namespace SuperSpace.Pages.Offce.MicrosoftOffice.Word;

internal sealed partial class WordPage : ListPage
{
    List<string> suffix_word = new List<string> { ".doc", ".docx"};
    public WordPage()
    {
        Icon = IconHelpers.FromRelativePath("Assets\\SuperSpaceWordPageIcon.png");
        Title = T("Office.Microsoft.WordPage.Title");
    }
    public override IListItem[] GetItems()
    {
        var items = new List<IListItem>
        {
            new ListItem(new OpenApp("Word.EXE"))
            {
                Title = T("Office.Microsoft.WordPage.CreateNewDoc"),
                Subtitle = T("Office.Microsoft.WordPage.CreateNewDocSub"),
                Icon = IconHelpers.FromRelativePath("Assets\\FluentColorDocumentAdd48.png"),
            }
        };
        items.AddRange(new RecentFile("Microsoft", "Office", "Recent", true, suffix_word)
            .items);
        return items.ToArray();
    }
}