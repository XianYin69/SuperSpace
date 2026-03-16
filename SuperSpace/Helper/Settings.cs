using Microsoft.CommandPalette.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSpace.Helper
{
    internal sealed partial class Settings : ICommandSettings
    {
        IContentPage ICommandSettings.SettingsPage => throw new NotImplementedException();
    }
}
