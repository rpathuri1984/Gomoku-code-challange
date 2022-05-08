namespace Gomoku.Logic;

public class GomokuGame : IGomokuGame
{
    #region Private Members

    public static readonly int WIN_STONES_COUNT = 5;

    private readonly Stack<Cell> _history;

    #endregion

    #region Public Props
    public GomokuBoard GomokuBoard { get; }
    public bool IsTie => _history.Count == MaxMove;
    public bool IsOver { get; private set; }
    public int MaxMove { get; }
    public PlayerManager Manager { get; }
    public Cell? LastMove => _history.Count == 0 ? null : _history.Peek();

    #endregion

    #region Constructor

    public GomokuGame()
    {
        IList<Player> players = new List<Player>()
                        {
                          new Player("Player 1", new Stone(Stones.X)),
                          new Player("Player 2", new Stone(Stones.O)),
                        };

        GomokuBoard = new GomokuBoard(15, 15);
        MaxMove = 15 * 15;
        Manager = new PlayerManager(players);
        _history = new Stack<Cell>();
        IsOver = false;
    }

    public GomokuGame(int width, int height, IEnumerable<Player> players)
    {
        if (height <= WIN_STONES_COUNT || width <= WIN_STONES_COUNT)
        {
            throw new ArgumentException(
              $"{nameof(GomokuGame)} must have at least {WIN_STONES_COUNT}x{WIN_STONES_COUNT} board.");
        }

        GomokuBoard = new GomokuBoard(width, height);
        MaxMove = width * height;
        Manager = new PlayerManager(players);
        _history = new Stack<Cell>();
        IsOver = false;
    }

    #endregion

    #region Public Methods

    public PlayResult PlaceStone(int x, int y)
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
            return new PlayResult { CurrentPlayer = Manager.CurrentPlayer.Name, IsGameOver = true, Message = "Game is already over" };
        }

        Cell cell = GomokuBoard[x, y];

        // Check for cell availability
        if (cell.Stone.Type != Stones.None)
        {
            return new PlayResult { CurrentPlayer = Manager.CurrentPlayer.Name, IsGameOver = false, Message = "Position is already filled" };
        }

        // place player stone on board and register this in history
        cell.Stone = Manager.CurrentPlayer.Stone;
        _history.Push(cell);

        // Check the game
        if (CheckIsGamveOver(x, y, out IList<Cell> winningLine))
        {
            IsOver = true;
            return new PlayResult { CurrentPlayer = Manager.CurrentPlayer.Name, IsGameOver = true, Message = $"Game is over. {Manager.CurrentPlayer.Name} wins.", WinningLine = winningLine };
        }

        // Increment turn for next player
        Manager.Turn.MoveNext();

        return new PlayResult { CurrentPlayer = Manager.CurrentPlayer.Name, IsGameOver = false, Message = "Game is not over" };
    }

    #endregion

    #region Private Methods

    private bool CheckIsGamveOver(int x, int y, out IList<Cell> winningCells)
    {
        if (IsOver || IsTie)
        {
            winningCells = new Cell[0];
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

        // to decide [win/game_over/tie], we require minimum stones 
        if (_history.Count < (WIN_STONES_COUNT * 2) - 1)
        {
            winningCells = new Cell[0];
            return false;
        }

        // read the recent cell of stone placed
        Cell cell = GomokuBoard[x, y];

        // verify in all 4 directions from the position that stone placed
        if (cell.Stone.Type != Stones.None)
        {
            Orientations[] orientations = new[]
            {
              Orientations.Horizontal,
              Orientations.Vertical,
              Orientations.Diagonal,
              Orientations.ReverseDiagonal
            };

            foreach (Orientations orientation in orientations)
            {
                // read orientedLine from board
                var orientedLine = GomokuOrientedLine.CollectStonesFromBoard(
                  GomokuBoard,
                  cell.X,
                  cell.Y,
                  cell.Stone,
                  orientation,
                  maxStones: WIN_STONES_COUNT);

                if (orientedLine.IsChained
                  && orientedLine.SameCellsCount + 1 == WIN_STONES_COUNT
                  )
                {
                    var result = orientedLine.GetSameCells().ToList();
                    result.Add(cell);
                    winningCells = result;
                    return true;
                }
            }
        }

        winningCells = new Cell[0];
        return false;
    }
    #endregion
}
