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
        private GomokuGame? _gomokuGame;
        private IList<Player> _players;
        #endregion

        #region Constructor

        public GomokuController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        #endregion


        [HttpGet]
        [Route("CrateBoard")]
        public IActionResult CrateBoard()
        {
            _players = new List<Player>()
                        {
                          new Player("Player 1", new Stone(Stones.X)),
                          new Player("Player 2", new Stone(Stones.O)),
                        };

            _gomokuGame = new GomokuGame(15, 15, _players);

            string gameKey = HashString($"{Guid.NewGuid()}{DateTime.Today.ToLongDateString()} {DateTime.Today.ToLongDateString()}");

            _memoryCache.Set(gameKey.ToString(), _gomokuGame);

            return Ok(new { gameKey });
        }

        [HttpGet]
        [Route("{gameKey}/PlaceStone")]
        public IActionResult PlaceStone(string gameKey, int x, int y)
        {

            _gomokuGame = _memoryCache.Get(gameKey) as GomokuGame;

            if (_gomokuGame != null)
            {
                var result = _gomokuGame.PlaceStone(x, y);

                _memoryCache.Set(gameKey, _gomokuGame);
                return Ok(result);
            }

            return BadRequest(new { ErrorMessage = "Game Not Initilized. use GET: /StartGame" });

        }

        [HttpGet]
        [Route("{gameKey}/CloseBoard")]
        public IActionResult CloseBoard(string gameKey)
        {
            _memoryCache.Remove(gameKey);
            return Ok();
        }

        private string HashString(string text, string salt = "")
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            // Uses SHA256 to create the hash
            using (var sha = new System.Security.Cryptography.SHA256Managed())
            {
                // Convert the string to a byte array first, to be processed
                byte[] textBytes = System.Text.Encoding.UTF8.GetBytes(text + salt);
                byte[] hashBytes = sha.ComputeHash(textBytes);

                // Convert back to a string, removing the '-' that BitConverter adds
                string hash = BitConverter
                    .ToString(hashBytes)
                    .Replace("-", String.Empty);

                return hash;
            }
        }
    }
}
