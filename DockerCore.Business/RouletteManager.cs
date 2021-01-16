using DockerCore.Business.Abstract;
using DockerCore.Cache.Abstract;
using DockerCore.Cross.Entities;
using DockerCore.Cross.Helpers;
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

        public async Task<Roulette> Add()
        {
            return await _rouletteService.Add();
        }

        public async Task<bool> Open(Roulette roulette)
        {
            return await _rouletteService.Open(roulette);
        }

        public async Task<ResultBet> Bet(BetRoulette betRoulette)
        {
            if (ValidateNumber(betRoulette) || ValidateColor(betRoulette))
            {
                var resultBetBd = await _rouletteService.Bet(betRoulette);

                if (resultBetBd)
                {
                    return new ResultBet { Succesfull = true, Message = MessageResource.OkBet };
                }
            }

            return new ResultBet { Succesfull = false, Message = MessageResource.ErrorDataBet };
        }

        private bool ValidateNumber(BetRoulette betRoulette)
        {
            if (betRoulette.Number >= 0 && betRoulette.Number <= 36)
            {
                return true;
            }

            return false;
        }

        private bool ValidateColor(BetRoulette betRoulette)
        {
            if (betRoulette.Color.Equals("ROJO", StringComparison.OrdinalIgnoreCase) || 
                betRoulette.Color.Equals("NEGRO", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return false;
        }
    }
}
