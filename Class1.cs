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
            for (byte i = 0; i < gg.Count; i++)
                try
                {

                    if (gg[i] == null || gg[i].playerID == null || gg[i].playerID.steamID == null) continue;
                    UnturnedPlayer player = UnturnedPlayer.FromCSteamID(gg[i].playerID.steamID);
#if DEBUG
                    if (player != null && player.Id != null) Rocket.Core.Logging.Logger.Log(player.Id.ToString() + " should bypass queue? (" + Provider.pending.Count.ToString() + ")"); else Rocket.Core.Logging.Logger.Log("Player is null");
#endif
                    if (player != null && player.HasPermission(Configuration.Instance.Permission))
                    { // nånstans här som det pajar, kanske
                        if (i > 2)
                        {
                            SteamPending pend = gg[i];
                            Provider.pending.RemoveAt(i);
                            Provider.pending.Insert(0,pend);
                            i--;
#if DEBUG
                            Rocket.Core.Logging.Logger.Log("Prepended " + player.Id.ToString());
#endif
                            //Provider.pending.RemoveAt(i);
                            continue;
                        }
#if DEBUG
                        Rocket.Core.Logging.Logger.Log("Sending verify packets to " + player.Id.ToString());
#endif
                        if (gg[i].canAcceptYet)
                        {
#if DEBUG
                            Rocket.Core.Logging.Logger.Log("Accepting " + player.Id.ToString());
#endif
                            Provider.accept(gg[i]);
                        }
                        gg[i].sendVerifyPacket();
                        gg[i].inventoryDetailsReady();
                    }
                }
                catch (Exception e) { Rocket.Core.Logging.Logger.LogException(e); }
        }
    }
}
