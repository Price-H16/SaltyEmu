﻿using ChickenAPI.Packets.Old.Attributes;

namespace ChickenAPI.Packets.Old.Game.Client.Mates
{
    [PacketHeader("ps_op")]
    public class PsopPacket : PacketBase
    {
        #region Properties

        [PacketIndex(0)]
        public byte PetSlot { get; set; }

        [PacketIndex(1)]
        public byte SkillSlot { get; set; }

        [PacketIndex(2)]
        public byte Option { get; set; }

        #endregion
    }
}