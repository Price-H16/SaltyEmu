﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ChickenAPI.Data.AccessLayer.Map;
using ChickenAPI.Data.TransferObjects.Map;
using Microsoft.EntityFrameworkCore;
using NosSharp.DatabasePlugin.Context;
using NosSharp.DatabasePlugin.Models.Map;
using NosSharp.DatabasePlugin.Services.Base;

namespace NosSharp.DatabasePlugin.Services.Map
{
    public class MapNpcDao : MappedRepositoryBase<MapNpcDto, MapNpcModel>, IMapNpcService
    {
        public MapNpcDao(NosSharpContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public IEnumerable<MapNpcDto> GetByMapId(long mapId)
        {
            try
            {
                return DbSet.Where(s => s.MapId == mapId).AsEnumerable().Select(Mapper.Map<MapNpcDto>);
            }
            catch (Exception e)
            {
                Log.Error("[GET_BY_MAP_ID]", e);
                throw;
            }

        }

        public async Task<IEnumerable<MapNpcDto>> GetByMapIdAsync(long mapId)
        {
            try
            {
                return (await DbSet.Where(s => s.MapId == mapId).ToArrayAsync()).Select(Mapper.Map<MapNpcDto>);
            }
            catch (Exception e)
            {
                Log.Error("[GET_BY_MAP_ID]", e);
                throw;
            }

        }
    }
}