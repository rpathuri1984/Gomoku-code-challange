namespace Gomoku.Logic;

public class Game
{
    #region Private Members

    public static readonly int WINPIECES = 5;

    private readonly Stack<Cell> _history;

    #endregion

    #region Public Props
    public GomokuBoard GomokuBoard { get; }
    public bool IsTie => _history.Count == MaxMove;
    public bool IsOver { get; private set; }
    public int MaxMove { get; }
    public PlayerManager Manager { get; }

    //public Tile? LastMove => _history.Count == 0 ? null : _history.Peek();

    #endregion

    #region Constructor
    public Game(int width, int height, IEnumerable<Player> players)
    {
        if (height <= WINPIECES || width <= WINPIECES)
        {
            throw new ArgumentException(
              $"{nameof(Game)} must have at least {WINPIECES}x{WINPIECES} board.");
        }

        GomokuBoard = new GomokuBoard(width, height);
        MaxMove = width * height;
        Manager = new PlayerManager(players);
        _history = new Stack<Cell>();
        IsOver = false;
    }

    #endregion

    #region Public Methods

    public PlayResult Play(int x, int y)
    {
        if (x < 0 || x > GomokuBoard.Width)
        {
            throw new ArgumentException("Value is out of range", nameof(x));
        }

        if (y < 0 || y > GomokuBoard.Height)
        {
            throw new ArgumentException("Value is out of range", nameof(y));
        }

        // Check if game is over
        if (IsOver)
        {
            return new PlayResult { CurrentPlayer = Manager.CurrentPlayer, IsGameOver = true, Message = "Game is already over" };
        }

        Cell tile = GomokuBoard[x, y];

        // Check for already placed tile
        if (tile.Stone.Type != Stones.None)
        {
            return new PlayResult { CurrentPlayer = Manager.CurrentPlayer, IsGameOver = false, Message = "Position is already filled" }; ;
        }

        Player oldPlayer = Manager.CurrentPlayer;
        tile.Stone = oldPlayer.Stone;
        _history.Push(tile);

        // Check for game over
        if (CheckGameOver(x, y, out IList<Cell> winningLine))
        {
            IsOver = true;
            return new PlayResult { CurrentPlayer = Manager.CurrentPlayer, IsGameOver = true, Message = "Game is over", WinningLine = winningLine };
        }

        // Increment turn
        Manager.Turn.MoveNext();

        return new PlayResult { CurrentPlayer = Manager.CurrentPlayer, IsGameOver = false, Message = "Game is not over" };
    }

    #endregion

    #region Private Methods

    private bool CheckGameOver(int x, int y, out IList<Cell> winningTiles)
    {
        if (IsOver || IsTie)
        {
            winningTiles = new Cell[0];
            return true;
        }

        if (x < 0 || x > GomokuBoard.Width)
        {
            throw new ArgumentException("Value is out of range", nameof(x));
        }

        if (y < 0 || y > GomokuBoard.Height)
        {
            throw new ArgumentException("Value is out of range", nameof(y));
        }

        if (_history.Count < 9)
        {
            winningTiles = new Cell[0];
            return false;
        }

        Cell tile = GomokuBoard[x, y];

        if (tile.Stone.Type != Stones.None)
        {
            Orientations[] orientations = new[]
            {
              Orientations.Horizontal,
              Orientations.Vertical,
              Orientations.Diagonal,
              Orientations.ReverseDiagonal
            };

            // verify in all 4 dorections
            foreach (Orientations orientation in orientations)
            {
                // read line from board
                var line = OrientedlLine.FromBoard(
                  GomokuBoard,
                  tile.X,
                  tile.Y,
                  tile.Stone,
                  orientation,
                  maxTile: WINPIECES,
                  blankTolerance: 0);

                if (line.IsChained
                  && line.SameCellsCount + 1 == WINPIECES
                  && line.BlockCellsCount < 2)
                {
                    var result = line.GetSameTiles().ToList();
                    result.Add(tile);
                    winningTiles = result;
                    return true;
                }
            }
        }

        winningTiles = new Cell[0];
        return false;
    }
    #endregion
}
