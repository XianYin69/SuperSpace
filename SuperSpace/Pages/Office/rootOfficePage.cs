using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using SuperSpace.Addition.PageSupport;
using SuperSpace.Pages.Offce.MicrosoftOffice.Excel;
using SuperSpace.Pages.Offce.MicrosoftOffice.PowerPoint;
using SuperSpace.Pages.Offce.MicrosoftOffice.Word;
using System.Collections.Generic;
using static SuperSpace.Addition.i18n.i18n;

namespace SuperSpace.Pages.Office
{
    internal sealed partial class rootOfficePage : ListPage
    {
        public rootOfficePage()
        {
            Icon = IconHelpers.FromRelativePath("Assets\\SuperSpace");
            Title = T("Office.Title");
        } 
        public override IListItem[] GetItems()
        {
            var Items = new List<IListItem>
            {
                new ListItem(new WordPage())
                {
                    Icon = IconHelpers.FromRelativePath("Assets\\SuperSpaceWordPageIcon.png"),
                    Title = T("Office.WordPage.Title")
                },
                new ListItem(new ExcelPage())
                {
                    Icon = IconHelpers.FromRelativePath("Assets\\SuperSpaceExcelPageIcon.png"),
                    Title = T("Office.ExcelPage.Title")
                },
                new ListItem(new PowerPointPage())
                {
                    Icon = IconHelpers.FromRelativePath("Assets\\SuperSpacePowerPointPageIcon.png"),
                    Title = T("Office.PowerPointPage.Title")
                }
            };
            Items.AddRange(new RecentFile("Microsoft", "Office", "Recent", false, new List<string> { }).items);
            return Items.ToArray();
        }
    }
}
