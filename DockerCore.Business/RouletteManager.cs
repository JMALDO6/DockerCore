using DockerCore.Business.Abstract;
using DockerCore.Cache.Abstract;
using DockerCore.Cross.Entities;
using DockerCore.Cross.Helpers;
using DockerCore.Data.Service.Abstract;
using Microsoft.Extensions.Configuration;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DockerCore.Business
{
    public class RouletteManager : IRouletteManager
    {
        #region Properties

        private const string ROULETTE_FILTER_KEY = "Roulette.Filter.{0}.{1}";
        private const int PAGE_SIZE = 4;
        private ICacheService _cacheService;
        private IRouletteService _rouletteService;
        private readonly IConfiguration _configuration;
        private readonly TimeSpan expirationTime;

        #endregion

        #region Public Methods

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="rouletteService"></param>
        /// <param name="cacheService"></param>
        /// <param name="configuration"></param>
        public RouletteManager(IRouletteService rouletteService, ICacheService cacheService, IConfiguration configuration)
        {
            _cacheService = cacheService;
            _rouletteService = rouletteService;
            _configuration = configuration;
            expirationTime = TimeSpan.FromSeconds(_configuration["RedisConfig:ExpirationTime"].ToInt());
        }

        /// <summary>
        /// Create roulette
        /// </summary>
        /// <returns></returns>
        public async Task<Roulette> AddRoulette()
        {
            return await _rouletteService.Add();
        }

        /// <summary>
        /// Open the roulette
        /// </summary>
        /// <param name="roulette"></param>
        /// <returns></returns>
        public async Task<bool> OpenRoulette(Roulette roulette)
        {
            return await _rouletteService.OpenRoulette(roulette);
        }

        /// <summary>
        /// Bet in the roulette
        /// </summary>
        /// <param name="betRoulette">Bet</param>
        /// <returns></returns>
        public async Task<ResultBet> BetInRoulette(BetRoulette betRoulette)
        {
            if (ValidateNumber(betRoulette) ||
                ValidateColor(betRoulette) &&
                ValidateDollars(betRoulette))
            {
                var resultBetBd = await _rouletteService.BetRoulette(betRoulette);
                if (resultBetBd)
                {
                    return new ResultBet { Succesfull = true, Message = MessageResource.OkBet };
                }
            }

            return new ResultBet { Succesfull = false, Message = MessageResource.ErrorDataBet };
        }

        /// <summary>
        /// Closed the roulette
        /// </summary>
        /// <param name="roulette"></param>
        /// <returns>List the bets</returns>
        public async Task<ResultRoulette> ClosedRoulette(Roulette roulette)
        {
            var result = await _rouletteService.ClosedRoulette(roulette);
            if (result)
            {
                var resultRoulette = new ResultRoulette
                {
                    ResultNumber = GenerateNumber()
                };
                var betsRoulette = await _rouletteService.GetBetsRoulette(roulette);
                resultRoulette.BetRoulettes = ProccesRoulette(betsRoulette, resultRoulette.ResultNumber);

                return resultRoulette;
            }

            return null;
        }

        /// <summary>
        /// List the roulettes
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<List<Roulette>> Search(int page)
        {
            string cacheKey = string.Format(ROULETTE_FILTER_KEY, page, PAGE_SIZE);
            var data = _cacheService.Get<List<Roulette>>(cacheKey);
            if (data != null)
            {
                return data;
            }
            data = await _rouletteService.Search(page, PAGE_SIZE);
            if (data != null)
            {
                _cacheService.Set(cacheKey, data, expirationTime);

                return data;
            }

            return null;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Generate number winner
        /// </summary>
        /// <returns>Number</returns>
        private int GenerateNumber()
        {
            var guid = Guid.NewGuid();
            var justNumbers = new string(guid.ToString().Where(Char.IsDigit).ToArray());
            var seed = int.Parse(justNumbers.Substring(0, 4));
            var random = new Random(seed);
            var value = random.Next(0, 36);

            return value;
        }

        private List<BetRoulette> ProccesRoulette(List<BetRoulette> lstBetRoulettes, int numberResult)
        {
            foreach (var betRoulette in lstBetRoulettes)
            {
                float earnedMoney = 0;

                if (betRoulette.Number != null && betRoulette.Number == numberResult)
                {
                    earnedMoney = betRoulette.Dollars * 5;
                }
                else if (betRoulette.Color != null && IsPar(numberResult) && betRoulette.Color.Equals("ROJO", StringComparison.OrdinalIgnoreCase))
                {
                    earnedMoney = betRoulette.Dollars * 1.8f;
                }
                else if (betRoulette.Color != null && !IsPar(numberResult) && betRoulette.Color.Equals("NEGRO", StringComparison.OrdinalIgnoreCase))
                {
                    earnedMoney = betRoulette.Dollars * 1.8f;
                }
                betRoulette.EarnedMoney = earnedMoney;
            }

            return lstBetRoulettes;
        }

        /// <summary>
        /// Valid number is par
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        private static bool IsPar(int number)
        {
            return Convert.ToBoolean(number % 2 == 0);
        }

        /// <summary>
        /// Validate value number
        /// </summary>
        /// <param name="betRoulette">Bet</param>
        /// <returns>Result validation. True = Correct</returns>
        private bool ValidateNumber(BetRoulette betRoulette)
        {
            if (betRoulette.Number != null && betRoulette.Number >= 0 && betRoulette.Number <= 36)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Validate value color
        /// </summary>
        /// <param name="betRoulette">Bet</param>
        /// <returns>Result validation. True = Correct</returns>
        private bool ValidateColor(BetRoulette betRoulette)
        {
            if (betRoulette.Color != null &&
                (betRoulette.Color.Equals("ROJO", StringComparison.OrdinalIgnoreCase) ||
                betRoulette.Color.Equals("NEGRO", StringComparison.OrdinalIgnoreCase)))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Validate amount of dollars
        /// </summary>
        /// <param name="betRoulette">Bet</param>
        /// <returns>Result validation. True = Correct</returns>
        private bool ValidateDollars(BetRoulette betRoulette)
        {
            if (betRoulette.Dollars > 0 && betRoulette.Dollars <= 10000)
            {
                return true;
            }

            return false;
        }

        #endregion
    }
}
