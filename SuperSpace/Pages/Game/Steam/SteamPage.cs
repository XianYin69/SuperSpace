using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using static SuperSpace.Addition.i18n.i18n;

namespace SuperSpace.Pages.Game.Steam
{
    internal sealed partial class SteamPage : ListPage {
        public SteamPage()
        {
            Icon = IconHelpers.FromRelativePath("Assets\\");
            Title = T("");
        }

        public override IListItem[] GetItems()
        {

        }
    }; 

}
