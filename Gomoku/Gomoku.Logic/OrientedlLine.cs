using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gomoku.Logic;

public class OrientedlLine
{

    public static readonly OrientedlLine EMPTY = new OrientedlLine();


    #region Public Props

    public Stone Stone { get; }
    public Orientations Orientation { get; }
    public DirectionalLine FirstLine { get; }
    public DirectionalLine SecondLine { get; }
    public ImmutableArray<DirectionalLine> Lines { get; }
    public int BlankCellsCount { get; }
    public int BlockCellsCount { get; }
    public int SameCellsCount { get; }
    public int CellsCount { get; }
    public bool IsChained { get; }

    #endregion

    private OrientedlLine() : this(
      (Stone)Stones.None,
      Orientations.None,
      DirectionalLine.EMPTY,
      DirectionalLine.EMPTY)
    {
    }

    private OrientedlLine(
      Stone stone,
      Orientations orientation,
      DirectionalLine firstLine,
      DirectionalLine secondLine)
    {
        Stone = stone;
        Orientation = orientation;
        FirstLine = firstLine;
        SecondLine = secondLine;

        Lines = ImmutableArray.Create(firstLine, secondLine);
        BlankCellsCount = firstLine.BlankCells.Length + secondLine.BlankCells.Length;
        BlockCellsCount = firstLine.BlockCells.Length + secondLine.BlockCells.Length;
        SameCellsCount = firstLine.SameCells.Length + secondLine.SameCells.Length;
        CellsCount = firstLine.Cells.Length + secondLine.Cells.Length;
        IsChained = firstLine.IsChained && secondLine.IsChained;
    }

    public static OrientedlLine FromBoard(
      GomokuBoard board,
      int x,
      int y,
      Stone type,
      Orientations orientation,
      int maxTile,
      int blankTolerance)
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

        if (maxTile < 0)
        {
            throw new ArgumentException(nameof(maxTile));
        }

        if (blankTolerance < 0)
        {
            throw new ArgumentException(nameof(blankTolerance));
        }

        return orientation switch
        {
            Orientations.Horizontal => new OrientedlLine(
              type,
              orientation,
              DirectionalLine.FromBoard(board, x, y, type, Directions.Left, maxTile, blankTolerance),
              DirectionalLine.FromBoard(board, x, y, type, Directions.Right, maxTile, blankTolerance)),
            Orientations.Vertical => new OrientedlLine(
              type,
              orientation,
              DirectionalLine.FromBoard(board, x, y, type, Directions.Up, maxTile, blankTolerance),
              DirectionalLine.FromBoard(board, x, y, type, Directions.Down, maxTile, blankTolerance)),
            Orientations.Diagonal => new OrientedlLine(
              type,
              orientation,
              DirectionalLine.FromBoard(board, x, y, type, Directions.UpLeft, maxTile, blankTolerance),
              DirectionalLine.FromBoard(board, x, y, type, Directions.DownRight, maxTile, blankTolerance)),
            Orientations.ReverseDiagonal => new OrientedlLine(
              type,
              orientation,
              DirectionalLine.FromBoard(board, x, y, type, Directions.UpRight, maxTile, blankTolerance),
              DirectionalLine.FromBoard(board, x, y, type, Directions.DownLeft, maxTile, blankTolerance)),
            _ => throw new InvalidOperationException("Unexpected value"),
        };
    }

    public IEnumerable<Cell> GetSameTiles()
    {
        return Lines.SelectMany(l => l.SameCells);
    }


}

