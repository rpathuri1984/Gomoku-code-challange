using System.Collections.Immutable;

namespace Gomoku.Logic;

public class GomokuOrientedLine
{

    public static readonly GomokuOrientedLine EMPTY = new GomokuOrientedLine();


    #region Public Props

    public ImmutableArray<GomokuDirectionalLine> Lines { get; }
    public int SameCellsCount { get; }
    public bool IsChained { get; }

    #endregion

    private GomokuOrientedLine() : this(
      GomokuDirectionalLine.EMPTY,
      GomokuDirectionalLine.EMPTY)
    {
    }

    private GomokuOrientedLine(
      GomokuDirectionalLine firstLine,
      GomokuDirectionalLine secondLine)
    {
        Lines = ImmutableArray.Create(firstLine, secondLine);
        SameCellsCount = firstLine.SameCells.Length + secondLine.SameCells.Length;
        IsChained = firstLine.IsChained && secondLine.IsChained;
    }

    public static GomokuOrientedLine CollectStonesFromBoard(
      GomokuBoard board,
      int x,
      int y,
      Stone currentStone,
      Orientations orientation,
      int maxStones)
    {
        if (board is null)
        {
            throw new ArgumentNullException(nameof(board));
        }

        if (x < 0 || x > board.Width)
        {
            throw new ArgumentException("Value is out of range", nameof(x));
        }

        if (y < 0 || y > board.Height)
        {
            throw new ArgumentException("Value is out of range", nameof(y));
        }

        if (maxStones < 0)
        {
            throw new ArgumentException(nameof(maxStones));
        }

        // collect stones from both directos where stone placed
        return orientation switch
        {
            Orientations.Horizontal => new GomokuOrientedLine(
              GomokuDirectionalLine.CollectStonesFromBoard(board, x, y, currentStone, Directions.Left, maxStones),
              GomokuDirectionalLine.CollectStonesFromBoard(board, x, y, currentStone, Directions.Right, maxStones)),
            Orientations.Vertical => new GomokuOrientedLine(
              GomokuDirectionalLine.CollectStonesFromBoard(board, x, y, currentStone, Directions.Up, maxStones),
              GomokuDirectionalLine.CollectStonesFromBoard(board, x, y, currentStone, Directions.Down, maxStones)),
            Orientations.Diagonal => new GomokuOrientedLine(
              GomokuDirectionalLine.CollectStonesFromBoard(board, x, y, currentStone, Directions.UpLeft, maxStones),
              GomokuDirectionalLine.CollectStonesFromBoard(board, x, y, currentStone, Directions.DownRight, maxStones)),
            Orientations.ReverseDiagonal => new GomokuOrientedLine(
              GomokuDirectionalLine.CollectStonesFromBoard(board, x, y, currentStone, Directions.UpRight, maxStones),
              GomokuDirectionalLine.CollectStonesFromBoard(board, x, y, currentStone, Directions.DownLeft, maxStones)),
            _ => throw new InvalidOperationException("Unexpected value"),
        };
    }

    public IEnumerable<Cell> GetSameCells()
    {
        return Lines.SelectMany(l => l.SameCells);
    }
}

