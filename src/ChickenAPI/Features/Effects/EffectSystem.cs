﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ChickenAPI.Core.ECS.Entities;
using ChickenAPI.Core.ECS.Systems;
using ChickenAPI.Core.ECS.Systems.Args;
using ChickenAPI.Game.Features.Effects.Args;
using ChickenAPI.Game.Maps;
using ChickenAPI.Game.Packets.Extensions;
using ChickenAPI.Game.Packets.Game.Client;

namespace ChickenAPI.Game.Features.Effects
{
    public class EffectSystem : NotifiableSystemBase
    {
        public EffectSystem(IEntityManager entityManager) : base(entityManager)
        {
        }

        /// <summary>
        /// Once per second
        /// </summary>
        protected override short RefreshRate => 1;

        protected override Expression<Func<IEntity, bool>> Filter => entity => entity.HasComponent<EffectComponent>();


        protected override void Execute(IEntity entity)
        {
            List<EffectPacket> packets = new List<EffectPacket>();
            var effects = entity.GetComponent<EffectComponent>();
            foreach (EffectComponent.Effect effect in effects.Effects)
            {
                if (effect.LastCast.AddMilliseconds(effect.Cooldown) > DateTime.UtcNow)
                {
                    continue;
                }

                effect.LastCast = DateTime.UtcNow;
                packets.Add(entity.GenerateEffectPacket(effect.Id));
                entity.GenerateInPacket();
            }

            if (entity.EntityManager is IMapLayer mapLayer)
            {
                mapLayer.Broadcast((IEnumerable<EffectPacket>)packets);
            }
        }

        public override void Execute(IEntity entity, SystemEventArgs e)
        {
            switch (e)
            {
                case AddEffectArgument addEffect:
                    AddEffectArgument(entity, addEffect);
                    break;
                default:
                    return;
            }
        }

        private void AddEffectArgument(IEntity entity, AddEffectArgument args)
        {
            var effects = entity.GetComponent<EffectComponent>();

            if (effects == null)
            {
                effects = new EffectComponent(entity);
                entity.AddComponent(effects);
            }

            effects.Effects.Add(new EffectComponent.Effect(args.EffectId, args.Cooldown));
        }
    }
}