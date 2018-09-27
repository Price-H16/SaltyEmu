﻿using ChickenAPI.Game.Entities;

namespace ChickenAPI.Game.Data.AccessLayer.Battle
{
    public interface IDamageAlgorithm
    {
        /// <summary>
        /// Returns the damages when one entity strikes another based on their stats
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="targetEntity"></param>
        /// <returns></returns>
        uint GenerateDamage(IBattleEntity entity, IBattleEntity targetEntity);
    }
}