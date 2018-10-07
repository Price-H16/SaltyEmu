﻿using System.Linq;
using ChickenAPI.Core.Logging;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Movements;
using ChickenAPI.Game.Movements.Events;
using ChickenAPI.Packets.Game.Client.Movement;

namespace NosSharp.PacketHandler.Move
{
    public class SitPacketHandling
    {
        private static readonly Logger Log = Logger.GetLogger<SitPacket>();

        public static void OnPacketReceived(SitPacket packet, IPlayerEntity player)
        {
            foreach (SitSubPacket u in packet.Users)
            {
                if (u.UserType == 1 && u.UserId != player.Character.Id)
                {
                    return;
                }

                //TODO: rest on mate
            }

            player.EmitEvent(new TriggerSitEvent
            {
                ChildsId = packet.Users.Select(s => s.UserId)
            });
        }
    }
}