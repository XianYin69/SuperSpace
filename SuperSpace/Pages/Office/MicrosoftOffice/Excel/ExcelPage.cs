//It is a new page of office-plus
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using System.Collections.Generic;
using static SuperSpace.Addition.i18n.i18n;
using SuperSpace.Addition.PageSupport;
using SuperSpace.Addition.OpenApp;

namespace SuperSpace.Pages.Offce.MicrosoftOffice.Excel;

//ExcelPage for open or creat a table file
internal sealed partial class ExcelPage : ListPage
{
    List<string> suffix_name = new List<string>() { ".xls", ".xlsx" };
    public ExcelPage()
    {
        Icon = IconHelpers.FromRelativePath("Assets\\SuperSpaceExcelPageIcon.png");
        Title = T("Office.Microsoft.ExcelPage.Title");
    }
    public override IListItem[] GetItems()
    {
        var items = new List<IListItem>
        {
            new ListItem(new OpenApp("EXCEL.EXE"))
            {
                Title = T("Office.Microsoft.ExcelPage.CreateNewDoc"),
                Subtitle = T("Office.Microsoft.ExcelPage.Subtitle"),
                Icon = IconHelpers.FromRelativePath("Assets\\FluentColorDocumentAdd48.png"),
            }
        };
        items.AddRange(new RecentFile("Microsoft", "Office", "Recent", true, suffix_name)
            .items);
        return items.ToArray();
    }
}