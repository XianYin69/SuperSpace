using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace SuperSpace.Addition.AppSupport.SteamSupport.AcfSupport
{
    public static class AcfSupport
    {
        private static string _AppId = string.Empty;
        private static string _Name = string.Empty;

        public static void Parse(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return;
            }
            using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var sr = new StreamReader(fs);
            string content = sr.ReadToEnd();

            _AppId = Regex.Match(content, @"""appid""\s+""([^""]+)""").Groups[1].Value;
            _Name = Regex.Match(content, @"""name""\s+""([^""]+)""").Groups[1].Value;
        }

        public static string LauncherPath(string filePath)
        {
            Parse(filePath);
            return $"steam://rungameid/{_AppId}";
        }

        public static string Name() => _Name ?? "Unknown Game";
    }
}
