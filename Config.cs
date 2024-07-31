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
        public byte PrependPosition;
        public byte MaxPlayers;
        public byte ReservedSlots;
        public int Interval;
        public string Permission;

        public void LoadDefaults()
        {
            sync = false;
            BypassMaxPlayers = false;
            PrependPosition = 2;
            MaxPlayers = 24;
            ReservedSlots = 48;
            Interval = 10;
            Permission = "bypass.queue";
        }
    }
}
