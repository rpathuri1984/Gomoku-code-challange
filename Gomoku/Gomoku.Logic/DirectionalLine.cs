namespace Gomoku.Logic;

public class DirectionalLine
{
    public static readonly DirectionalLine EMPTY = new DirectionalLine();


    #region Public Props

    public Cell[] Cells { get; }
    public Cell[] BlankCells { get; }
    public Cell[] BlockCells { get; }
    public Cell[] SameCells { get; }
    public Directions Direction { get; }
    public bool IsChained { get; }
    public Stone Stone { get; }
    #endregion


    private DirectionalLine() : this(
      (Stone)Stones.None,
      Directions.None,
      new Cell[0],
      new Cell[0],
      new Cell[0],
      new Cell[0],
      false)
    {
    }

    private DirectionalLine(
      Stone stone,
      Directions diretion,
      Cell[] tiles,
      Cell[] sameTiles,
      Cell[] blankTiles,
      Cell[] blockTiles,
      bool isChained)
    {
        Stone = stone;
        Direction = diretion;
        Cells = tiles;
        SameCells = sameTiles;
        BlankCells = blankTiles;
        BlockCells = blockTiles;
        IsChained = isChained;
    }


    public static DirectionalLine FromBoard(
    GomokuBoard gomokuBoard,
    int x,
    int y,
    Stone stone,
    Directions direction,
    int maxCell,
    int blankTolerance)
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

        if (maxCell < 0)
        {
            throw new ArgumentException(nameof(maxCell));
        }

        if (blankTolerance < 0)
        {
            throw new ArgumentException(nameof(blankTolerance));
        }

        var tiles = new Queue<Cell>();
        var sameTiles = new Queue<Cell>();
        var blankTiles = new Queue<Cell>();
        var blockTiles = new Queue<Cell>();

        var count = 0;
        var chainBreak = false;
        var blank = 0;

        gomokuBoard.IterateTiles(
          x,
          y,
          direction,
          t =>
          {
              if (count++ == maxCell)
              {
                  return false;
              }

              if (t.Stone.Type == stone)
              {
                  if (blank > 0)
                  {
                      chainBreak = true;
                  }

                  tiles.Enqueue(t);
                  sameTiles.Enqueue(t);
                  return true;
              }
              else if (t.Stone.Type == Stones.None)
              {
                  if (blank++ == blankTolerance)
                  {
                      return false;
                  }

                  tiles.Enqueue(t);
                  blankTiles.Enqueue(t);
                  return true;
              }
              else
              {
                  tiles.Enqueue(t);
                  blockTiles.Enqueue(t);
                  return false;
              }
          });

        return new DirectionalLine(
          stone,
          direction,
          tiles.ToArray(),
          sameTiles.ToArray(),
          blankTiles.ToArray(),
          blockTiles.ToArray(),
          !chainBreak);
    }
}


