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
            Rocket.Core.Logging.Logger.Log("Loading");
            if(Configuration.Instance.sync) StartCoroutine(nameof(loop));
            else Offload = Task.Run(() => StartCoroutine(nameof(loop)));
            if(Configuration.Instance.ReservedSlots != 0 && Configuration.Instance.MaxPlayers != 0)
            {
                Rocket.Unturned.U.Events.OnPlayerDisconnected += PDC;
            }
            Rocket.Core.Logging.Logger.Log("Loaded");
        }
        protected override void Unload()
        {
            StopAllCoroutines();
            if (!Configuration.Instance.sync) Offload.Dispose();
            if (Configuration.Instance.ReservedSlots != 0 && Configuration.Instance.MaxPlayers != 0)
            {
                Rocket.Unturned.U.Events.OnPlayerDisconnected -= PDC;
            }
            Rocket.Core.Logging.Logger.Log("Unloaded");
        }
        void PDC(UnturnedPlayer player)
        {
            if (Provider.maxPlayers > Configuration.Instance.MaxPlayers) Provider.maxPlayers = (byte)Provider.clients.Count;
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
            //List<SteamPending> gg = Provider.pending;
            for (byte i = 0; i < Provider.pending.Count; i++)
                try
                {
                    if (Provider.pending[i] == null || Provider.pending[i].playerID.steamID == null) continue;
                    RocketPlayer player = new RocketPlayer(Provider.pending[i].playerID.steamID.ToString());

                    if (player != null && player.HasPermission(Configuration.Instance.Permission))
                    {
                        if (Provider.clients.Count >= Provider.maxPlayers)
                        {
                            if (Configuration.Instance.BypassMaxPlayers || Configuration.Instance.ReservedSlots > Provider.maxPlayers) { Provider.maxPlayers += 1; } else { PrependQue(ref i); continue; }
                        }
                        if (Provider.pending[i].canAcceptYet)
                        {
#if DEBUG
                            Rocket.Core.Logging.Logger.Log("Accepting " + i);
#endif
                            Provider.accept(Provider.pending[i]);
                            continue;
                        }
#if DEBUG
                            Rocket.Core.Logging.Logger.Log("Sending verify packets to " + i);
#endif
                        PrependQue(ref i);
                        Provider.pending[i].sendVerifyPacket();
                        Provider.pending[i].inventoryDetailsReady();
                    }
                }
                catch (Exception e) { Rocket.Core.Logging.Logger.LogException(e); }
        }
        void PrependQue(ref byte i)
        {
            if (i > Configuration.Instance.PrependPosition)
            {
                Provider.pending.Insert(0, Provider.pending[i]);
                Provider.pending.RemoveAt(i + 1);
                // i--;
                // Unneccessary in this case
#if DEBUG
                            Rocket.Core.Logging.Logger.Log("Prepended " + i);
#endif
                //Provider.pending.RemoveAt(i);
            }
        }
    }
}
