﻿using ChickenAPI.Packets.ServerPackets.Player;

namespace ChickenAPI.Game.Entities.Player.Extensions
{
    public static class CInfoPacketExtension
    {
        public static CInfoPacket GenerateCInfoPacket(this IPlayerEntity player) =>
            new CInfoPacket
            {
                Name = player.Character.Name,
                Unknown1 = "-", //TODO: Find signification
                GroupId = -1,
                FamilyId = (int)(player.Family?.Id ?? -1),
                FamilyName = player.Family?.Name ?? "-",
                CharacterId = player.Character.Id,
                Authority = (byte)player.NameAppearance,
                Gender = player.Character.Gender,
                HairStyle = player.Character.HairStyle,
                HairColor = player.Character.HairColor,
                Class = player.Character.Class,
                Icon = (byte)player.GetReputIcon(), // todo
                Compliment = player.Character.Compliment,
                Morph = player.MorphId,
                Invisible = player.IsInvisible,
                FamilyLevel = 0,
                MorphUpgrade = player.Sp?.Upgrade ?? 0,
                ArenaWinner = player.Character.ArenaWinner
            };
    }
}