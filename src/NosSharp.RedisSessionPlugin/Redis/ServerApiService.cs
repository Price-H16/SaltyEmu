﻿using System;
using System.Collections.Generic;
using System.Linq;
using ChickenAPI.Data.AccessLayer.Server;
using ChickenAPI.Data.TransferObjects.Server;
using ChickenAPI.Utils;
using Foundatio.Caching;
using StackExchange.Redis;

namespace NosSharp.RedisSessionPlugin.Redis
{
    public class ServerApiService : IServerApiService
    {
        private static readonly Logger Log = Logger.GetLogger<ServerApiService>();
        private const string Prefix = nameof(WorldServerDto) + "_";
        private const string AllKeys = Prefix + "*";
        private RedisConfiguration _configuration;
        private readonly ICacheClient _cache;

        public ServerApiService(RedisConfiguration config)
        {
            _configuration = config;

            var options = new RedisCacheClientOptions
            {
                ConnectionMultiplexer = ConnectionMultiplexer.Connect(config.Host)
            };
            _cache = new RedisCacheClient(options);
        }

        private static string ToKey(WorldServerDto dto) => ToKey(dto.Id);

        private static string ToKey(Guid id) => Prefix + id;


        public bool RegisterServer(WorldServerDto dto)
        {
            IDictionary<string, CacheValue<WorldServerDto>> servers = _cache.GetAllAsync<WorldServerDto>(AllKeys).GetAwaiter().GetResult();
            if (servers.Values.Any(s => s.Value.Id == dto.Id))
            {
                Log.Warn("Server with the same Guid is already registered");
                return true;
            }

            _cache.AddAsync(ToKey(dto), dto);
            return false;
        }

        public void UnregisterServer(Guid id)
        {
            _cache.RemoveAllAsync(new[] { ToKey(id) });
        }

        public IEnumerable<WorldServerDto> GetServers()
        {
            return _cache.GetAllAsync<WorldServerDto>(AllKeys).GetAwaiter().GetResult().Where(s => s.Value.HasValue).Select(s => s.Value.Value);
        }
    }
}