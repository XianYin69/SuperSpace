using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using SuperSpace.Addition.AppSupport.SteamSupport.AcfSupport;
using SuperSpace.Addition.AppSupport.SteamSupport.VdfSupport;
using SuperSpace.Addition.OpenApp;
using System.IO;
using System.Linq;

namespace SuperSpace.Addition.AppSupport.SteamSupport

//It is a supported addition to add items in List
{
    internal sealed partial class SteamSupport
    {
        public List<IListItem> items { get; } = new();
        public SteamSupport()
        {
            try
            {
                //use VdfSupport to get the path of library
                var vdfProvider = new VdfSupport.VdfSupport(@"HKEY_CURRENT_USER\Software\Valve\Steam", "libraryfolders.vdf", "steamapps");
                //process acf file
                var targetDirectories = vdfProvider.LibraryPaths
                    .Select(path => Path.Combine(path, "steamapps"))
                    .Where(Directory.Exists)
                    .ToList();
                //collect and process this acf
                var allAcfFiles = targetDirectories
                    .SelectMany(dir => Directory.GetFiles(dir, "*.acf"))
                    .ToList();
                //Add this items to list
                foreach (var file in allAcfFiles)
                {
                    string launchUrl = AcfSupport.AcfSupport.LauncherPath(file);
                    string gameName = AcfSupport.AcfSupport.Name();
                    if (!string.IsNullOrEmpty(launchUrl))
                    {
                        items.Add(new ListItem(new OpenApp.OpenApp(launchUrl, true))
                        {
                            Title = gameName
                        });
                    }
                };
            } catch (Exception) {

            }
        }
    }
}