using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace SuperSpace.Pages.SuperSpaceSettingPage
{
    internal sealed partial class SuperSpaceSettingPage : ContentPage
    {
        private readonly Settings _settings = new();
        private readonly List<ChoiceSetSetting.Choice> _choices = new()
        {
            new ChoiceSetSetting.Choice("SteamCommonPath",""),
            new ChoiceSetSetting.Choice("EpicGamePath","")
        };
        
        public override IContent[] GetContent()
        {
            var s = _settings.ToContent();
            return s;
        }

        public SuperSpaceSettingPage()
        {
            Name = "SuperSpaceSettings";
            Icon = new IconInfo(string.Empty);
            _settings.Add(new TextSetting("SteamGamePath", "")
            {
                Label = "Steam Common Folder Path:",
                Description = ""
            });
            _settings.Add(new TextSetting("EpicGamePath", "")
            {
                Label = "Epic Game Folder Path:",
                Description = ""
            });
        }
    }
}
