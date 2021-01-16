using DockerCore.Business.Abstract;
using DockerCore.Cross.Entities;
using DockerCore.Cross.Helpers;
using DockerCore.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace DockerCore.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RouletteController : ControllerBase
    {
        private readonly IRouletteManager _rouletteManager;

        public RouletteController(IRouletteManager rouletteManager)
        {
            _rouletteManager = rouletteManager;
        }

        /// <summary>
        /// Create the roulette
        /// </summary>
        /// <returns>Roulette created</returns>
        [HttpPost]
        public async Task<IActionResult> Create()
        {
            try
            {
                var roulette = await _rouletteManager.Add();
                if (roulette is null)
                {
                    return StatusCode(500);
                }

                return StatusCode(201, roulette);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        /// <summary>
        /// Open the roulette
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Open(int id)
        {
            try
            {
                var roulette = new Roulette { Id = id };
                var isUpdated = await _rouletteManager.Open(roulette);

                if (!isUpdated)
                {
                    return StatusCode(403, new ErrorModel { ErrorMessage = string.Format(MessageResource.ErrorNotFound, id) });
                }

                return StatusCode(201, new { Success = true });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        /// <summary>
        /// Bet in the roulette
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Bet(BetRoulette betRoulette)
        {
            try
            {
                var resultBet = await _rouletteManager.Bet(betRoulette);

                if (!resultBet.Succesfull)
                {
                    return StatusCode(403, resultBet);
                }

                return StatusCode(201, resultBet);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        /// <summary>
        /// Close the roulette
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("{Close}")]
        public IActionResult Close()
        {
            return Ok();
        }

        /// <summary>
        /// List of roulettes
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("{List}")]
        public IActionResult List()
        {
            return Ok();
        }
    }
}
