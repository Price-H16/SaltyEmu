﻿using ChickenAPI.Enums.Packets;
using ChickenAPI.Game.Data.TransferObjects.Skills;
using ChickenAPI.Game.Entities;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Features.Skills;
using ChickenAPI.Game.Features.Skills.Args;
using ChickenAPI.Packets.Game.Server.QuickList.Battle;
using ChickenAPI.Game.Features.Movement;
using ChickenAPI.Game.Features.Movement.Extensions;

namespace ChickenAPI.Game.Features.Battle.Extensions
{
    public static class BattleExtensions
    {
        public static void TargetHit(this IBattleEntity entity, IBattleEntity target, long skillId)
        {
            var skillComponent = entity.Battle.Entity.GetComponent<SkillComponent>();
            var movableComponent = entity.Battle.Entity.GetComponent<MovableComponent>();
            var targetMovableComponent = entity.Battle.Entity.GetComponent<MovableComponent>();
            if (!(entity.Battle.Entity is IPlayerEntity player))
            {
                player = null;
            }
            SkillDto skill = skillComponent.Skills[skillId];
            if (skill == null || SkillEventHandler.TryCastSkill(skillComponent, new SkillCastArgs { Skill = skill }))
            {
                player?.SendPacket(new CancelPacket { Type = CancelPacketType.InCombatMode, TargetId = target.Battle.Entity.Id });
                return;
            }
            switch (skill.TargetType)
            {
                // Single Hit
                case 0:
                    if (movableComponent.GetDistance(targetMovableComponent) > skill.Range + target.Battle.BasicArea + 1)
                    {
                        goto default;
                    }
                    entity.NotifyEventHandler<SkillEventHandler>(new UseSkillArgs { Skill = skill });

                    break;

                // AOE Target Hit
                case 1 when skill.HitType == 1:
                    break;

                // AOE Buff
                case 1 when skill.HitType != 1:
                    break;

                // Buff
                case 2 when skill.HitType == 0:
                    break;

                default:
                    player?.SendPacket(new CancelPacket { Type = CancelPacketType.InCombatMode, TargetId = target.Battle.Entity.Id });
                    return;
            }
        }
    }
}