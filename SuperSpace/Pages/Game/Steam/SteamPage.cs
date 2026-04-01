using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using SuperSpace.Addition.OpenApp;
using static SuperSpace.Addition.i18n.i18n;
using static SuperSpace.Addition.OpenApp.OpenApp;
using SuperSpace.Addition.AppSupport.SteamSupport;

namespace SuperSpace.Pages.Game.Steam
{
    internal sealed partial class SteamPage : ListPage {
        public SteamPage()
        {
            Icon = IconHelpers.FromRelativePath("Assets\\SuperSpaceSteamPageIcon.png");
            Title = T("");
        }

        public override IListItem[] GetItems()
        {
            string steamDir = Microsoft.Win32.Registry.GetValue(@"HKEY_CURRENT_USER\Software\Valve\Steam", "SteamPath", null) as string ?? "";
            string steamExecutePath = Path.Combine(steamDir, "steam.exe");
            var Items = new List<IListItem>
            {
                new ListItem(new OpenApp(steamExecutePath, false))
                {
                    Icon = IconHelpers.FromRelativePath("Assets\\SuperSpaceSteamPageIcon.png"),
                    Title = T("")
                }
            };
            Items.AddRange(new SteamSupport().items);
            return Items.ToArray();
        }
    }; 

}
