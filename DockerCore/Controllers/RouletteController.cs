using DockerCore.Business.Abstract;
using DockerCore.Cross.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create()
        {
            var isCreated = await _rouletteManager.Add(new Roulette());
            if (!isCreated)
            {
                return StatusCode(500);
            }

            return StatusCode(201);
        }

        /// <summary>
        /// Open the roulette
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("{Open}")]
        public IActionResult Open()
        {
            return Ok();
        }

        /// <summary>
        /// Bet in the roulette
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("{Bet}")]
        public IActionResult Bet()
        {
            return Ok();
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
