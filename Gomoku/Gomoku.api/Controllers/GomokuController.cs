using Gomoku.Logic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Gomoku.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GomokuController : ControllerBase
    {
        #region Private Fields

        private readonly IMemoryCache _memoryCache;
        private Game? _gomokuGame;
        private IList<Player> _players;
        #endregion

        #region Constructor

        public GomokuController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        #endregion


        [HttpGet]
        [Route("StartGame")]
        public IActionResult StartGame()
        {
            _players = new List<Player>()
                        {
                          new Player("Player 1", new Stone(Stones.X)),
                          new Player("Player 2", new Stone(Stones.O)),
                        };

            _gomokuGame = new Game(15, 15, _players);

            Guid gameKey = Guid.NewGuid();
            _memoryCache.Set(gameKey.ToString(), _gomokuGame);

            return Ok(new { gameKey });
        }

        [HttpGet]
        [Route("{gameKey}/Play/{x}/{y}")]
        public IActionResult Play(string gameKey, int x, int y)
        {

            _gomokuGame = _memoryCache.Get(gameKey) as Game;

            if (_gomokuGame != null)
            {
                var result = _gomokuGame.Play(x, y);

                _memoryCache.Set(gameKey, _gomokuGame);
                return Ok(result);
            }

            return BadRequest(new { ErrorMessage = "Game Not Initilized. use GET: /StartGame" });

        }

        [HttpGet]
        [Route("{gameKey}/Close")]
        public IActionResult CloseGame(string gameKey)
        {
            _memoryCache.Remove(gameKey);
            return Ok();
        }
    }
}
