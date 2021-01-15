using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DockerCore.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RouletteController : ControllerBase
    {
        /// <summary>
        /// Create the roulette
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Create")]
        public IActionResult Create()
        {
            return Ok();
        }

        /// <summary>
        /// Open the roulette
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Open")]
        public IActionResult Open()
        {
            return Ok();
        }

        /// <summary>
        /// Bet in the roulette
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Bet")]
        public IActionResult Bet()
        {
            return Ok();
        }

        /// <summary>
        /// Close the roulette
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Close")]
        public IActionResult Close()
        {
            return Ok();
        }

        /// <summary>
        /// List of roulettes
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("List")]
        public IActionResult List()
        {
            return Ok();
        }
    }
}
