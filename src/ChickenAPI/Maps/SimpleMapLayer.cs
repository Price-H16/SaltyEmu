﻿using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using ChickenAPI.Core.ECS.Entities;
using ChickenAPI.Core.ECS.Systems;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Utils;
using ChickenAPI.Game.Data.AccessLayer.Shop;
using ChickenAPI.Game.Data.TransferObjects.Map;
using ChickenAPI.Game.Data.TransferObjects.Shop;
using ChickenAPI.Game.Entities.Monster;
using ChickenAPI.Game.Entities.Npc;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Entities.Portal;
using ChickenAPI.Game.Features.Chat;
using ChickenAPI.Game.Features.Effects;
using ChickenAPI.Game.Features.Inventory;
using ChickenAPI.Game.Features.Movement;
using ChickenAPI.Game.Features.Shops;
using ChickenAPI.Game.Features.Visibility;
using ChickenAPI.Game.Packets;

namespace ChickenAPI.Game.Maps
{
    public class SimpleMapLayer : EntityManagerBase, IMapLayer
    {
        public SimpleMapLayer(IMap map, IEnumerable<MapMonsterDto> monsters, IEnumerable<MapNpcDto> npcs = null, IEnumerable<PortalDto> portals = null, IEnumerable<ShopDto> shops = null)
        {
            Id = Guid.NewGuid();
            Map = map;
            ParentEntityManager = map;
            var movable = new MovableSystem(this);
            NotifiableSystems = new Dictionary<Type, INotifiableSystem>
            {
                { typeof(VisibilitySystem), new VisibilitySystem(this) },
                { typeof(ChatSystem), new ChatSystem(this) },
                { typeof(MovableSystem), movable },
                { typeof(InventorySystem), new InventorySystem(this) },
                { typeof(ShopSystem), new ShopSystem(this) },
                { typeof(EffectSystem), new EffectSystem(this) }
            };
            AddSystem(movable);
            if (monsters != null)
            {
                foreach (MapMonsterDto monster in monsters)
                {
                    RegisterEntity(new MonsterEntity(monster));
                }
            }

            if (npcs != null)
            {
                foreach (MapNpcDto npc in npcs)
                {
                    ShopDto shop = shops.FirstOrDefault(s => s.MapNpcId == npc.Id);
                    RegisterEntity(new NpcEntity(npc, shop));
                }
            }


            if (portals != null)
            {
                foreach (PortalDto portal in portals)
                {
                    RegisterEntity(new PortalEntity(portal));
                }
            }


            StartSystemUpdate();
        }

        public Guid Id { get; set; }
        public IMap Map { get; }

        public IEnumerable<IEntity> GetEntitiesInRange(Position<short> pos, int range) =>
            Entities.Where(e => e.HasComponent<MovableComponent>() && PositionHelper.GetDistance(pos, e.GetComponent<MovableComponent>().Actual) < range);

        public void Broadcast<T>(T packet) where T : IPacket
        {
            foreach (IPlayerEntity i in GetEntitiesByType<IPlayerEntity>(EntityType.Player))
            {
                i.SendPacket(packet);
            }
        }

        public void Broadcast<T>(IPlayerEntity sender, T packet) where T : IPacket
        {
            foreach (IPlayerEntity i in GetEntitiesByType<IPlayerEntity>(EntityType.Player))
            {
                if (i == sender)
                {
                    continue;
                }

                i.SendPacket(packet);
            }
        }
    }
}