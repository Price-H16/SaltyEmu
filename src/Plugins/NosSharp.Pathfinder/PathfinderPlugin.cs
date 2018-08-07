﻿using ChickenAPI.Core.Plugins;
using ChickenAPI.Core.Logging;
using System;
using ChickenAPI.Core.IoC;
using Autofac;

namespace NosSharp.Pathfinder
{
    public class PathfinderPlugin : IPlugin
    {
        private static readonly Logger Log = Logger.GetLogger<PathfinderPlugin>();
        public string Name => nameof(PathfinderPlugin);

        public void OnLoad()
        {
            Log.Info($"Loading...");
            Container.Builder.Register(s => new Pathfinders.Pathfinder()).As<IPathfinder>().SingleInstance();
            Log.Info($"Loaded !");
        }

        public void ReloadConfig()
        {
            throw new NotImplementedException();
        }

        public void SaveConfig()
        {
            throw new NotImplementedException();
        }

        public void SaveDefaultConfig()
        {
            throw new NotImplementedException();
        }


        public void OnDisable()
        {
        }

        public void OnEnable()
        {
        }
    }
}