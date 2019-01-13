﻿using System.Threading;
using System.Threading.Tasks;
using ChickenAPI.Core.Events;
using ChickenAPI.Data.Character;
using ChickenAPI.Game.Effects;
using ChickenAPI.Game.Entities.Extensions;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Entities.Player.Events;
using ChickenAPI.Game.Entities.Player.Extensions;

namespace SaltyEmu.BasicPlugin.EventHandlers
{
    public class Player_LevelUp_Handler : GenericEventPostProcessorBase<PlayerLevelUpEvent>
    {
        private readonly IAlgorithmService _algorithm;

        public Player_LevelUp_Handler(IAlgorithmService algorithm)
        {
            _algorithm = algorithm;
        }

        protected override async Task Handle(PlayerLevelUpEvent e, CancellationToken cancellation)
        {

            if (!(e.Sender is IPlayerEntity player))
            {
                return;
            }

            player.HpMax = _algorithm.GetHpMax(player.Character.Class, player.Level);
            player.Hp = player.HpMax;
            player.MpMax = _algorithm.GetMpMax(player.Character.Class, player.Level);
            player.Mp = player.MpMax;
            await player.SendPacketAsync(player.GenerateLevPacket());
            await player.SendPacketAsync(player.GenerateStatPacket());
            await player.SendPacketAsync(player.GenerateLevelUpPacket());
            switch (e.LevelUpType)
            {
                case LevelUpType.Level:
                    player.Broadcast(player.GenerateEffectPacket(6));
                    break;
                case LevelUpType.HeroLevel:
                case LevelUpType.JobLevel:
                    player.Broadcast(player.GenerateEffectPacket(8));
                    break;
            }

            player.Broadcast(player.GenerateEffectPacket(198));
        }
    }
}