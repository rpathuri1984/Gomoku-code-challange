namespace Gomoku.Logic;

public class GomokuBoard : IGomokuBoard
{
    #region Private Members

    private readonly Cell[,] _cells;

    #endregion

    #region Public Props

    public int Height { get; }
    public int Width { get; }

    public Cell this[int x, int y] => _cells[x, y];

    #endregion

    #region Constructor
    public GomokuBoard(int width, int height)
    {
        if (height < 0 || width < 0)
        {
            throw new ArgumentException($"{nameof(GomokuBoard)} must have non-negative dimensions.");
        }

        Width = width;
        Height = height;
        _cells = new Cell[Width, Height];
        for (var i = 0; i < Width; i++)
        {
            for (var j = 0; j < Height; j++)
            {
                _cells[i, j] = new Cell(i, j)
                {
                    Stone = (Stone)Stones.None,
                };
            }
        }
    }
    #endregion

    public void IterateCells(
      int x,
      int y,
      Directions direction,
      Predicate<Cell> predicate,
      bool iterateSelf = false)
    {
        if (x < 0 || x > Width)
        {
            throw new ArgumentOutOfRangeException(nameof(x));
        }

        if (y < 0 || y > Height)
        {
            throw new ArgumentOutOfRangeException(nameof(y));
        }

        if (predicate is null)
        {
            throw new ArgumentNullException(nameof(predicate));
        }

        var startingOffset = iterateSelf ? 0 : 1;

        // look for same stone in 8 directions by traversing up to max_win_stones_count. ex. 5
        switch (direction)
        {
            case Directions.Left:
                for (int i = x - startingOffset, j = y;
                    i >= 0 && predicate(_cells[i, j]);
                    i--)
                {
                }

                break;

            case Directions.Right:
                for (int i = x + startingOffset, j = y;
                    i < Width && predicate(_cells[i, j]);
                    i++)
                {
                }

                break;

            case Directions.Up:
                for (int i = x, j = y - startingOffset;
                    j >= 0 && predicate(_cells[i, j]);
                    j--)
                {
                }

                break;

            case Directions.Down:
                for (int i = x, j = y + startingOffset;
                    j < Height && predicate(_cells[i, j]);
                    j++)
                {
                }

                break;

            case Directions.UpLeft:
                for (int i = x - startingOffset, j = y - startingOffset;
                    i >= 0 && j >= 0 && predicate(_cells[i, j]);
                    i--, j--)
                {
                }

                break;

            case Directions.DownRight:
                for (int i = x + startingOffset, j = y + startingOffset;
                    i < Width && j < Height && predicate(_cells[i, j]);
                    i++, j++)
                {

                }

                break;

            case Directions.UpRight:
                for (int i = x + startingOffset, j = y - startingOffset;
                    i < Width && j >= 0 && predicate(_cells[i, j]);
                    i++, j--)
                {
                }

                break;

            case Directions.DownLeft:
                for (int i = x - startingOffset, j = y + startingOffset;
                    i >= 0 && j < Height && predicate(_cells[i, j]);
                    i--, j++)
                {
                }

                break;

            default:
                throw new ArgumentException("Value is not supported.", nameof(direction));
        }
    }

}

