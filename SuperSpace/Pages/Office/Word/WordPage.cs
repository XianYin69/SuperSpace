//It is a new page of office-plus
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;
using SuperSpace.Addition.i18n;
using static SuperSpace.Addition.i18n.i18n;
using SuperSpace.Addition.PageSupport;
using static SuperSpace.Addition.PageSupport.RecentFile;

namespace SuperSpace.Pages;

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
            },
            new ListItem(new RecentFile("Microsoft","Office","Recent", true, suffix_word))
            {
                Title = T("WordPage.OpenDoc"),
                Subtitle = T("WordPage.OpenDocSub"),
                Icon = IconHelpers.FromRelativePath("Assets\\FluentColorDocumentEdit24.png"),
            }
        };
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