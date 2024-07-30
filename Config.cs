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
        public int Interval;
        public string Permission;

        public void LoadDefaults()
        {
            sync = false;
            Interval = 30;
            Permission = "bypass.queue";
        }
    }
}
