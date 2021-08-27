using System;
using System.Collections.Generic;
using Unity;
using UnityEngine;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MEC;

namespace _001WDBPlugin
{
    public class Class1 : Plugin<Config>
    {
        bool broadcastSent = false;
        public override void OnEnabled()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers += RollRound;
            Log.Debug(@"|⠀⠀⠀
░░░░▄▄▄▄▀▀▀▀▀▀▀▀▄▄▄▄▄▄
░░░░█░░░░▒▒▒▒▒▒▒▒▒▒▒▒░░▀▀▄
░░░█░░░▒▒▒▒▒▒░░░░░░░░▒▒▒░░█
░░█░░░░░░▄██▀▄▄░░░░░▄▄▄░░░█
░▀▒▄▄▄▒░█▀▀▀▀▄▄█░░░██▄▄█░░░█
█▒█▒▄░▀▄▄▄▀░░░░░░░░█░░░▒▒▒▒▒█
█▒█░█▀▄▄░░░░░█▀░░░░▀▄░░▄▀▀▀▄▒█
░█▀▄░█▄░█▀▄▄░▀░▀▀░▄▄▀░░░░█░░█
░░█░░▀▄▀█▄▄░█▀▀▀▄▄▄▄▀▀█▀██░█
░░░█░░██░░▀█▄▄▄█▄▄█▄████░█
░░░░█░░░▀▀▄░█░░░█░███████░█
░░░░░▀▄░░░▀▀▄▄▄█▄█▄█▄█▄▀░░█
░░░░░░░▀▄▄░▒▒▒▒░░░░░░░░░░█
░░░░░░░░░░▀▀▄▄░▒▒▒▒▒▒▒▒▒▒░█
░░░░░░░░░░░░░░▀▄▄▄▄▄░░░░░█⠀⠀⠀⠀⠀⠀⠀
|
|
|                        that yoinky sploinky");
            base.OnEnabled();
        }
        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers -= RollRound;
            Exiled.Events.Handlers.Player.SyncingData -= OnSyncingData;
            Exiled.Events.Handlers.Warhead.ChangingLeverStatus -= OnChangingLeverStatus;
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
            Exiled.Events.Handlers.Server.RespawningTeam -= OnRespawningTeam;
            Exiled.Events.Handlers.Player.ChangingRole -= OnChangingRole;
            Exiled.Events.Handlers.Player.Died -= OnDied;
            base.OnDisabled();
        }

        //TODO add an onDisabled thing

        private void RollRound()
        {
            if (UnityEngine.Random.Range(0, 101) > Config.Chance)
            {
                Exiled.Events.Handlers.Player.SyncingData -= OnSyncingData;
                Exiled.Events.Handlers.Warhead.ChangingLeverStatus -= OnChangingLeverStatus;
                Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
                Exiled.Events.Handlers.Server.RespawningTeam -= OnRespawningTeam;
                Exiled.Events.Handlers.Player.ChangingRole -= OnChangingRole;
                Exiled.Events.Handlers.Player.Died -= OnDied;
                return;
            }
            Exiled.Events.Handlers.Player.SyncingData += OnSyncingData;
            Exiled.Events.Handlers.Warhead.ChangingLeverStatus += OnChangingLeverStatus;
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
            Exiled.Events.Handlers.Server.RespawningTeam += OnRespawningTeam;
            Exiled.Events.Handlers.Player.ChangingRole += OnChangingRole;
            Exiled.Events.Handlers.Player.Died += OnDied;
        }

        void OnSyncingData(SyncingDataEventArgs ev)
        {
            Player plr = ev.Player;

            if (plr.Role == RoleType.Scp0492)
                return;

            if (plr.CurrentRoom.Zone != Exiled.API.Enums.ZoneType.Surface)
                return;

            switch (plr.Team)
            {
                case Team.MTF:
                    return;
                case Team.SCP:
                    return;
                case Team.RIP:
                    return;
                case Team.TUT:
                    return;
            }

            if (plr.Team == Team.CHI && Config.CISuit || plr.HasItem(ItemType.ArmorHeavy))
                return;

            plr.Hurt(1 / Time.fixedDeltaTime * 0.1f);
        }
        void OnRoundStarted()
        {
            foreach (Lift lift in Lift.Instances)
            {
                lift.Lock();
            }
            if(Config.OverrideTickets)
            {
                Respawn.GrantTickets(Respawning.SpawnableTeamType.ChaosInsurgency, -Respawn.ChaosTickets, true);
                Respawn.GrantTickets(Respawning.SpawnableTeamType.NineTailedFox, -Respawn.NtfTickets, true);
            }
            
            Timing.CallDelayed(10, () => CustomAnnouncement("attention all personnel . x k class event scp 0 0 1 a k a when day breaks detected . .g5 . keep any biological materials inside the facility until arrival of an nato_e 11 escort team . . g3", "Attention all personnel: XK-Class event SCP 001 AKA 'When Day Breaks' detected. Keep any biological materials inside the facility until arrival of an E-11 escort team"));
            Timing.CallDelayed(90, () => CustomAnnouncement(".g3 .g4 .g6 .g2 attention all personnel . an up date from the O5 console . . there is no threat . the day is great . .g4 .g2 . go outside", "Attention all personnel: An update from the O5 council:  there is no threat  the day is great            go outside"));
        }
        void OnChangingLeverStatus(ChangingLeverStatusEventArgs ev)
        {
            if(ev.CurrentState == false && !broadcastSent)
            {
                broadcastSent = true;
                Respawn.GrantTickets(Respawning.SpawnableTeamType.NineTailedFox, 50, false);
                CustomAnnouncement("broadcast to o 5 console engaged .g6 .g4 .g1 .g3 . . . . . .g2 emergency broadcast complete .g6", "Broadcast to O5 Council engaged..........Emergency broadcast complete.");
                if(Config.OverrideTickets)
                {
                    Timing.CallDelayed(60, () => CustomAnnouncement("attention all personnel . an escort team is on the way . please locate the nearest evacuation shelter and close the gates once all survivors are secure", "Attention all personnel: An escort team is on the way. Please locate the nearest evacuation shelter and close the gates once all survivors are secure."));
                    Timing.CallDelayed(180, () => Respawn.ForceWave(Respawning.SpawnableTeamType.NineTailedFox, false));
                }
            }
        }
        void OnRespawningTeam(RespawningTeamEventArgs ev)
        {
            Timing.CallDelayed(1f, () =>
            {
                bool zombie = (UnityEngine.Random.Range(0, 101) < Config.ZombieChance);
                foreach (Player plr in ev.Players)
                {
                    if(zombie)
                    {
                        plr.SetRole(RoleType.Scp0492, Exiled.API.Enums.SpawnReason.None, true);
                        plr.Broadcast(10, "You have spawned as an SCP-001-A. Show them the light.", Broadcast.BroadcastFlags.Normal, true);
                    }
                    else if(ev.NextKnownTeam == Respawning.SpawnableTeamType.NineTailedFox)
                    {
                        plr.Broadcast(10, "You are E11 Escort team. Give scientists and handcuffed Class-D heavy armor to shield them as they go to surface. Your suit will protect you without the armor, so don't be afraid to share.", Broadcast.BroadcastFlags.Normal, true);
                    }
                }
            });
        }
        void OnChangingRole(ChangingRoleEventArgs ev)
        {
            Player plr = ev.Player;
            if (ev.Reason == Exiled.API.Enums.SpawnReason.Escaped)
                Timing.CallDelayed(0.2f, () =>
                {
                    List<ItemType> evitems = ev.Items;
                    evitems.Remove(ItemType.ArmorCombat);
                    plr.ResetInventory(evitems);
                    plr.AddItem(ItemType.ArmorHeavy);
                    plr.Heal(1000, false);
                });
        }
        void OnDied(DiedEventArgs ev)
        {
            Player plr = ev.Target;
            if(plr.CurrentRoom.Zone == Exiled.API.Enums.ZoneType.Surface && plr.Role != RoleType.Scp0492)
            {
                Timing.CallDelayed(1, () => plr.SetRole(RoleType.Scp0492, Exiled.API.Enums.SpawnReason.Died, false));
            }
        }
        void CustomAnnouncement(string cassieMessage, string broadcastMessage)
        {
            Cassie.Message(cassieMessage, true, false);
            foreach(Player plr in Player.List)
            {
                plr.Broadcast(20, broadcastMessage, Broadcast.BroadcastFlags.Normal, true);
            }
        }
    }
}
