using DockerCore.Business.Abstract;
using DockerCore.Cache.Abstract;
using DockerCore.Cross.Entities;
using DockerCore.Data.Service.Abstract;
using Microsoft.Extensions.Configuration;
using ServiceStack;
using System;
using System.Threading.Tasks;

namespace DockerCore.Business
{
    public class RouletteManager : IRouletteManager
    {
        private ICacheService _cacheService;
        private IRouletteService _rouletteService;
        private readonly IConfiguration _configuration;
        private readonly TimeSpan expirationTime;

        public RouletteManager(IRouletteService rouletteService, ICacheService cacheService, IConfiguration configuration)
        {
            _cacheService = cacheService;
            _rouletteService = rouletteService;
            _configuration = configuration;
            expirationTime = TimeSpan.FromSeconds(_configuration["RedisConfig:ExpirationTime"].ToInt());
        }

        public async Task<bool> Add(Roulette model)
        {
            return await _rouletteService.Add(model);
        }
    }
}
