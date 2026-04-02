using System.Collections.Generic;
using Microsoft.CommandPalette.Extensions;
using System.IO;
using Microsoft.Win32;
using System.Text.RegularExpressions;

namespace SuperSpace.Addition.AppSupport.SteamSupport.VdfSupport;

//It is a addition to scan file(.vdf) to add itmes to list

internal sealed partial class VdfSupport
{
    public List<string> LibraryPaths { get; } = new();
    public VdfSupport(string registryPath, string fileName, string supFolderName)
    {
        //Root Path of Software
        string SoftwarePath = Registry.GetValue(registryPath, "SteamPath", null) as string ?? string.Empty;
        if (string.IsNullOrEmpty(SoftwarePath))
        {
            return;
        };
        //Path of .vdf file
        string VdfPath = Path.Combine(SoftwarePath, supFolderName, fileName);
        //Parse it
        if (File.Exists(VdfPath))
        {
            try
            {
                using var fs = new FileStream(VdfPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                using var sr = new StreamReader(fs);
                string content = sr.ReadToEnd();

                var matches = Regex.Matches(content, @"""path""\s+""([^""]+)""");
                foreach (Match match in matches)
                {
                    string rawPath = match.Groups[1].Value;
                    string normalizedPath = rawPath.Replace("\\\\", "\\");
                    if (Directory.Exists(normalizedPath))
                    {
                        LibraryPaths.Add(normalizedPath);
                    }
                }
            } catch
            {

            }
        }
    }
}
