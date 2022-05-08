using Gomoku.Logic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Cryptography;

namespace Gomoku.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GomokuController : ControllerBase
    {
        #region Private Fields

        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<GomokuController> _logger;
        private IGomokuGame? _gomokuGame;
        private readonly IServiceProvider _serviceProvider;
        #endregion

        #region Constructor

        public GomokuController(IMemoryCache memoryCache, ILogger<GomokuController> logger,IServiceProvider serviceProvider)
        {
            _memoryCache = memoryCache;
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        #endregion


        [HttpGet]
        [Route("CrateBoard")]
        public IActionResult CrateBoard()
        {
            _gomokuGame = _serviceProvider.GetService<IGomokuGame>();

            // generating unique string to register game object in memory
            string gameKey = HashString($"{Guid.NewGuid()}{DateTime.Today.ToLongDateString()} {DateTime.Today.ToLongTimeString()}");

            _memoryCache.Set(gameKey.ToString(), _gomokuGame);

            return Ok(new { gameKey });
        }

        [HttpGet]
        [Route("{gameKey}/PlaceStone")]
        public IActionResult PlaceStone(string gameKey, int x, int y)
        {
            _gomokuGame = _memoryCache.Get(gameKey) as IGomokuGame;

            if (_gomokuGame != null)
            {
                var result = _gomokuGame.PlaceStone(x, y);
                _memoryCache.Set(gameKey, _gomokuGame);
                return Ok(result);
            }

            _logger.LogError($"Board having id[{gameKey}] not intialized.");
            return BadRequest(new { ErrorMessage = $"Game Not Initilized. use GET: /CrateBoard" });

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
            using (var sha = SHA256.Create())
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
