namespace Gomoku.Logic;

public class GomokuDirectionalLine
{
    public static readonly GomokuDirectionalLine EMPTY = new GomokuDirectionalLine();


    #region Public Props

    public Cell[] Cells { get; }
    public Cell[] SameCells { get; }
    public Directions Direction { get; }
    public bool IsChained { get; }
    public Stone Stone { get; }

    #endregion

    #region Constructor

    private GomokuDirectionalLine() : this(
      (Stone)Stones.None,
      Directions.None,
      new Cell[0],
      new Cell[0],
      false)
    {
    }

    private GomokuDirectionalLine(
      Stone stone,
      Directions diretion,
      Cell[] cells,
      Cell[] sameCells,
      bool isChained)
    {
        Stone = stone;
        Direction = diretion;
        Cells = cells;
        SameCells = sameCells;
        IsChained = isChained;
    }

    #endregion

    public static GomokuDirectionalLine CollectStonesFromBoard(
    GomokuBoard gomokuBoard,
    int x,
    int y,
    Stone stone,
    Directions direction,
    int maxStones)
    {
        if (gomokuBoard is null)
        {
            throw new ArgumentNullException(nameof(gomokuBoard));
        }

        if (x < 0 || x > gomokuBoard.Width)
        {
            throw new ArgumentOutOfRangeException(nameof(x));
        }

        if (y < 0 || y > gomokuBoard.Height)
        {
            throw new ArgumentOutOfRangeException(nameof(y));
        }

        if (maxStones < 0)
        {
            throw new ArgumentException(nameof(maxStones));
        }

        var cells = new Queue<Cell>();
        var sameCells = new Queue<Cell>();

        var count = 0;
        var chainBreak = false;

        // check the stones for given direction either end of the side of the board 
        // or same stones found.
        gomokuBoard.IterateCells(
          x,
          y,
          direction,
          cell =>
          {
              if (count++ == maxStones)
              {
                  return false;
              }

              if (cell.Stone.Type == stone)
              {
                  cells.Enqueue(cell);
                  sameCells.Enqueue(cell);
                  return true;
              }
              else if (cell.Stone.Type == Stones.None)
              {
                  cells.Enqueue(cell);
                  return true;
              }
              else
              {
                  cells.Enqueue(cell);
                  return false;
              }
          });

        return new GomokuDirectionalLine(
          stone,
          direction,
          cells.ToArray(),
          sameCells.ToArray(),
          !chainBreak);
    }
}