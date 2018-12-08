﻿using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Inventory.Events;
using ChickenAPI.Packets.Game.Client.Inventory;

namespace NosSharp.PacketHandler.Inventory
{
    public class MviPacketHandling
    {
        public static void OnMviPacket(MviPacket packet, IPlayerEntity player)
        {
            player.EmitEvent(new InventoryMoveEventArgs
            {
                InventoryType = packet.InventoryType,
                Amount = packet.Amount,
                SourceSlot = packet.InventorySlot,
                DestinationSlot = packet.DestinationSlot
            });
        }
    }
}