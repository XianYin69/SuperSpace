using System.Collections.Generic;
using Microsoft.CommandPalette.Extensions;
using System.IO;
using Microsoft.Win32;

namespace SuperSpace.Addition.VdfSupport;

//It is a addition to scan file(.vdf) to add itmes to list

internal sealed partial class VdfSupport
{
    public List<IListItem> items { get; } = new();
    public VdfSupport(string registryPath, string fileName, string supFolderName)
    {
        //Root Path of Software
        string SoftwarePath = (string)Registry.GetValue(registryPath, null);
        //Path of .vdf file
        string VdfPath = Path.Combine(SoftwarePath, supFolderName, fileName);
        //search items in .vdf file
        foreach (var subPath in ParseLibraryPaths(VdfPath))
        {
            var mainfestFiles = Directory.GetFiles(Path.Combine(subPath, supFolderName), "");
            foreach(var file in mainfestFiles)
            {
                var itme = ParseMainfest(file);
                if (itme != null) items.Add(itme);
            }
            items.ToArray();
        }
    }
}
