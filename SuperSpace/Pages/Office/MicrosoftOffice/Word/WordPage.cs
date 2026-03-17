//It is a new page of office-plus
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using System.Collections.Generic;
using System.Diagnostics;
using static SuperSpace.Addition.i18n.i18n;
using SuperSpace.Addition.PageSupport;

namespace SuperSpace.Pages.Offce.MicrosoftOffice.Word;

internal sealed partial class WordPage : ListPage
{
    List<string> suffix_word = new List<string> { ".doc", ".docx"};
    public WordPage()
    {
        Icon = IconHelpers.FromRelativePath("Assets\\SuperSpaceWordPageIcon.png");
        Title = "Word";
        Name = "";
    }
    public override IListItem[] GetItems()
    {
        var items = new List<IListItem>
        {
            new ListItem(new RunWordCommand("WINWORD.EXE"))
            {
                Title = T("WordPage.CreateNewDoc"),
                Subtitle = T("WordPage.CreateNewDocSub"),
                Icon = IconHelpers.FromRelativePath("Assets\\FluentColorDocumentAdd48.png"),
            }
        };
        items.AddRange(new RecentFile("Microsoft", "Office", "Recent", true, suffix_word)
            .items);
        return items.ToArray();
    }
}
internal sealed partial class RunWordCommand : InvokableCommand
{
    private readonly string _executable;
    public RunWordCommand(string executable)
    {
        _executable = executable;
    }

    public override CommandResult Invoke()
    {
        try
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = _executable,
                UseShellExecute = true
            });
            return CommandResult.Dismiss();
        }
        catch
        {
            return CommandResult.KeepOpen();
        }
    }
}