using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using static SuperSpace.Addition.GameFinder.Steam;
using static SuperSpace.Addition.GameFinder.Epic;

namespace SuperSpace.Addition.GameFinder
{
    internal class GameFinder
    {
        public List<IListItem> items { get; } = new(); 
        public GameFinder(string SoftPath)
        {
            
            if (SoftPath.SequenceEqual("steam"))
            {
                        
            } else if (SoftPath.SequenceEqual("Epic"))
            {

            } else
            {
                items.Add(new ListItem(new NoOpCommand()) {
                    Title = "",
                    Subtitle = ""
                });
            }
        }
    }
}
