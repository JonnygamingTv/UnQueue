using Rocket.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnQueue
{
    public class Config : IRocketPluginConfiguration
    {
        public bool sync;
        public bool BypassMaxPlayers;
        public int Interval;
        public byte PrependPosition;
        public string Permission;

        public void LoadDefaults()
        {
            sync = false;
            BypassMaxPlayers = false;
            Interval = 30;
            PrependPosition = 2;
            Permission = "bypass.queue";
        }
    }
}
