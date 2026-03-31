using System.Collections.Generic;
using Microsoft.CommandPalette.Extensions;
using System.IO;
using Microsoft.Win32;

namespace SuperSpace.Addition.AppSupport.SteamSupport.VdfSupport;

//It is a addition to scan file(.vdf) to add itmes to list

internal sealed partial class VdfSupport
{
    public VdfSupport(string registryPath, string fileName, string supFolderName)
    {
        //Root Path of Software
        string SoftwarePath = (string)Registry.GetValue(registryPath, null, null);
        //Path of .vdf file
        string VdfPath = Path.Combine(SoftwarePath, supFolderName, fileName);
    }
}
