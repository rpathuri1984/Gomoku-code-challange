using Gomoku.Logic;
using Microsoft.AspNetCore.Mvc;

namespace Gomoku.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GomokuRedisController : ControllerBase
    {
        #region Private Fields

        private readonly IRedisCacheUtility _redisCacheUtility;
            private readonly ILogger<GomokuController> _logger;
        private IGomokuGame? _gomokuGame;
        private readonly IServiceProvider _serviceProvider;
        #endregion

        #region Constructor

        public GomokuRedisController(IRedisCacheUtility redisCacheUtility, ILogger<GomokuController> logger, IServiceProvider serviceProvider)
        {
            _redisCacheUtility = redisCacheUtility;
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        #endregion


        [HttpGet]
        [Route("CrateBoard")]
        public async Task<IActionResult> CrateBoard()
        {
            _gomokuGame = _serviceProvider.GetService<IGomokuGame>();

            // generating unique string to register game object in cache
            string gameKey = _redisCacheUtility.GenerateHashKey($"{Guid.NewGuid()}{DateTime.Today.ToLongDateString()} {DateTime.Today.ToLongTimeString()}");

            // serialize the board and save in Redis cache
            await _redisCacheUtility.Set(gameKey.ToString(), _gomokuGame);

            return Ok(new { gameKey });
        }

        [HttpGet]
        [Route("{gameKey}/PlaceStone")]
        public async Task<IActionResult> PlaceStone(string gameKey, int x, int y)
        {
            // read game from cache
            _gomokuGame = await _redisCacheUtility.Get<IGomokuGame>(gameKey);

            if (_gomokuGame != null)
            {
                var result = _gomokuGame.PlaceStone(x, y);
                await _redisCacheUtility.Set(gameKey.ToString(), _gomokuGame);
                return Ok(result);
            }

            _logger.LogError($"Board having id[{gameKey}] not intialized.");
            return BadRequest(new { ErrorMessage = $"Game Not Initilized. use GET: /CrateBoard" });

        }

        [HttpGet]
        [Route("{gameKey}/CloseBoard")]
        public IActionResult CloseBoard(string gameKey)
        {
            // remove game
            _redisCacheUtility.Remove(gameKey);
            return Ok();
        }
    }
}
