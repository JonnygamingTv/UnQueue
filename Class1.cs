using Rocket.API;
using Rocket.Core.Plugins;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace UnQueue
{
    public class Class1 : RocketPlugin<Config>, IRocketPlayer
    {
        Task Offload;
        public string Id => throw new NotImplementedException();

        public string DisplayName => throw new NotImplementedException();

        public bool IsAdmin => throw new NotImplementedException();

        public int CompareTo(object obj)
        {
            throw new NotImplementedException();
        }

        protected override void Load()
        {
            Offload = Task.Run(() =>
            {
                StartCoroutine(nameof(loop));
            });
        }
        protected override void Unload()
        {
            StopAllCoroutines();
            Offload.Dispose();
        }
        private IEnumerator loop()
        {
            yield return new WaitForSeconds(Configuration.Instance.Interval);
            CheckQueue();
            StartCoroutine(nameof(loop));
        }

        void oCC() {
#if DEBUG
            Rocket.Core.Logging.Logger.Log("Client connected! (" + Provider.pending.Count.ToString() + ")");
#endif
            CheckQueue();
        }

        void CheckQueue()
        {
            List<SteamPending> gg = Provider.pending;
#if DEBUG
            Rocket.Core.Logging.Logger.Log("Looping through " + gg.Count.ToString());
#endif
            for (byte i = 0; i < gg.Count; i++)
            {
#if DEBUG
                Rocket.Core.Logging.Logger.Log(gg[i].playerID.ToString() + " should bypass queue? (" + Provider.pending.Count.ToString() + ")");
#endif
                if (!gg[i].canAcceptYet) continue;
                UnturnedPlayer player = UnturnedPlayer.FromCSteamID(gg[i].playerID.steamID);
#if DEBUG
                Rocket.Core.Logging.Logger.Log(player.Id.ToString() + " should bypass queue? (" + Provider.pending.Count.ToString() + ") x2");
#endif
                if (player.HasPermission(Configuration.Instance.Permission)) Provider.accept(gg[i]);
            }
        }
    }
}
