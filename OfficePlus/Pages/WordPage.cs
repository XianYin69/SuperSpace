//It is a new page of office-plus
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;

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
        return [
            new ListItem(new NoOpCommand()) { Title = "这是一个用于增强Microsoft Word功能的扩展。" },
            new ListItem(new NoOpCommand()) { Title = "功能包括模板管理、快捷操作等。" }
        ];
    }
}