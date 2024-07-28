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
        public int Interval;
        public string Permission;

        public void LoadDefaults()
        {
            Interval = 30;
            Permission = "bypass.queue";
        }
    }
}
